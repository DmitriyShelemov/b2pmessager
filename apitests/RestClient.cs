using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
namespace apitests
{
    public static class RestClient
    {
        public static async Task<T> PostAsync<T>(this HttpClient client, Uri uri, dynamic payload, HttpStatusCode code = HttpStatusCode.Created) where T : class
        {
            var apiResponse = await client.PostAsync(uri, JsonContent.Create(payload));

            Assert.IsNotNull(apiResponse);
            Assert.That(apiResponse.StatusCode, Is.EqualTo(code), await apiResponse.Content.ReadAsStringAsync());

            if (code != HttpStatusCode.Created)
                return null;

            var json = await apiResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task PutAsync(this HttpClient client, Uri uri, dynamic payload, HttpStatusCode code = HttpStatusCode.NoContent)
        {
            var apiResponse = await client.PutAsync(uri, JsonContent.Create(payload));

            Assert.IsNotNull(apiResponse);
            Assert.That(apiResponse.StatusCode, Is.EqualTo(code), await apiResponse.Content.ReadAsStringAsync());
        }

        public static async Task DeleteAsync(this HttpClient client, Uri uri, HttpStatusCode code = HttpStatusCode.NoContent)
        {
            var apiResponse = await client.DeleteAsync(uri);

            Assert.IsNotNull(apiResponse);
            Assert.That(apiResponse.StatusCode, Is.EqualTo(code), await apiResponse.Content.ReadAsStringAsync());
        }

        public static async Task<T> GetAsync<T>(this HttpClient client, Uri uri, HttpStatusCode code = HttpStatusCode.OK) where T : class
        {
            var apiResponse = await client.GetAsync(uri);

            Assert.IsNotNull(apiResponse);
            Assert.That(apiResponse.StatusCode, Is.EqualTo(code), await apiResponse.Content.ReadAsStringAsync());

            if (code != HttpStatusCode.OK)
                return null;

            var json = await apiResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
