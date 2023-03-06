using AutoCompare;
using facadeservice.Dto;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using static apitests.Routes;

namespace apitests
{
    [TestFixture]
    public class ApiTests : BaseApiTests
    {
        [Test]
        public async Task TenantTest()
        {
            var client = GetClient();
            var tenant = await client.PostAsync<TenantDto>(TenantUri, new TenantDto
            {
                Name = "MyTenant",
                Email = "mymail1@test.co"
            });
            Assert.IsNotNull(tenant.TenantUID);

            tenant = await client.GetAsync<TenantDto>(GetTenantUri(tenant.TenantUID));
            Assert.That(tenant.Name, Is.EqualTo("MyTenant"));

            await client.PutAsync(GetTenantUri(tenant.TenantUID), new TenantDto
            {
                Name = "MyTenantUpdated",
                Email = "mymail2@test.co"
            });
            var tenants = await client.GetAsync<List<TenantDto>>(TenantUri);
            Assert.That(tenants.Last().Name, Is.EqualTo("MyTenantUpdated"));

            await client.DeleteAsync(GetTenantUri(tenant.TenantUID));
            await client.GetAsync<TenantDto>(GetTenantUri(tenant.TenantUID), HttpStatusCode.NotFound);
        }

        [Test]
        public async Task ChatTest()
        {
            var client = GetClient();
            var tenant = await BuildTenantAsync(client);

            var createObj = new ChatCreateDto
            {
                Name = nameof(ChatDto)
            };

            var updateObj = new ChatCreateDto
            {
                Name = $"{nameof(ChatDto)}Updated"
            };

            await CrudTest<ChatDto, ChatDto>(client, tenant.TenantUID, createObj, updateObj, x => x.ChatUID, GetChatUri, GetChatUri);
        }

        [Test]
        public async Task MessageTest()
        {
            var client = GetClient();
            var tenant = await BuildTenantAsync(client);
            var chat = await BuildChatAsync(client, tenant);

            var createObj = new MessageCreateDto
            {
                ChatUID = chat.ChatUID,
                MessageText = "May street"
            };

            var updateObj = new MessageUpdateDto
            {
                MessageText = "May street1"
            };

            await CrudChildTest<MessageDto, MessageDto>(client,
                tenant.TenantUID, chat.ChatUID,
                createObj, updateObj,
                x => x.MessageUID,
                GetMessageManyUri, GetMessagePostUri, GetMessageExactUri,
                s => s.ChatUID = chat.ChatUID );
        }

        private static async Task<ChatDto> BuildChatAsync(HttpClient client, TenantDto tenant)
        {
            var chat = await RestClient.PostAsync<ChatDto>(client, GetChatUri(tenant.TenantUID), new ChatCreateDto
            {
                Name = nameof(ChatDto) + Guid.NewGuid().ToString("N")
            });
            Assert.IsNotNull(chat.ChatUID);
            return chat;
        }

        private static async Task<TenantDto> BuildTenantAsync(HttpClient client)
        {
            var tenant = await client.PostAsync<TenantDto>(TenantUri, new TenantDto
            {
                Name = "MyTenant" + Guid.NewGuid().ToString("N"),
                Email = "mymail2@test.co"
            });
            Assert.IsNotNull(tenant.TenantUID);
            return tenant;
        }

        public async Task CrudTest<T, B>(
            HttpClient client,
            Guid tenantUID,
            object createObj,
            object updateObj,
            Func<T, Guid> getUID,
            Func<Guid, Uri> getUri,
            Func<Guid, Guid, Uri> getExactUri,
            Action<B>? preprocessRefs = null) where T : class where B : class
        {
            var entity = await client.PostAsync<T>(getUri(tenantUID), createObj);
            Assert.IsNotNull(getUID(entity));

            entity = await client.GetAsync<T>(getExactUri(tenantUID, getUID(entity)));
            preprocessRefs?.Invoke(createObj as B);
            AssertCompared(Comparer.Compare<B>(createObj as B, entity as B));

            await client.PutAsync(getExactUri(tenantUID, getUID(entity)), updateObj);
            var entities = await client.GetAsync<List<T>>(getUri(tenantUID));
            preprocessRefs?.Invoke(updateObj as B);
            AssertCompared(Comparer.Compare<B>(updateObj as B, entities.Last() as B));

            await RestClient.DeleteAsync(client, getExactUri(tenantUID, getUID(entity)));
            await client.GetAsync<T>(getExactUri(tenantUID, getUID(entity)), HttpStatusCode.NotFound);
        }

        public async Task CrudChildTest<T, B>(
            HttpClient client,
            Guid tenantUID,
            Guid parentUID,
            object createObj,
            object updateObj,
            Func<T, Guid> getUID,
            Func<Guid, Guid, Uri> getManyUri,
            Func<Guid, Uri> getPostUri,
            Func<Guid, Guid, Uri> getExactUri,
            Action<B>? preprocessRefs = null) where T : class where B : class
        {
            var entity = await client.PostAsync<T>(getPostUri(tenantUID), createObj);
            Assert.IsNotNull(getUID(entity));

            entity = await client.GetAsync<T>(getExactUri(tenantUID, getUID(entity)));
            preprocessRefs?.Invoke(createObj as B);
            AssertCompared(Comparer.Compare<B>(createObj as B, entity as B));

            await client.PutAsync(getExactUri(tenantUID, getUID(entity)), updateObj);
            var entities = await client.GetAsync<List<T>>(getManyUri(tenantUID, parentUID));
            preprocessRefs?.Invoke(updateObj as B);
            AssertCompared(Comparer.Compare<B>(updateObj as B, entities.Last() as B));

            await RestClient.DeleteAsync(client, getExactUri(tenantUID, getUID(entity)));
            await client.GetAsync<T>(getExactUri(tenantUID, getUID(entity)), HttpStatusCode.NotFound);
        }

        private void AssertCompared(IList<Difference> differences)
        {
            Assert.That(differences.Count, Is.EqualTo(0), JsonConvert.SerializeObject(differences));
        }

        [OneTimeSetUp]
        public void SetupComparer()
        {
            AutoCompareConfig.Configure();
        }
    }
}