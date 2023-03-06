using Microsoft.Extensions.Configuration;

namespace apitests
{
    public abstract class BaseApiTests
    {
        private static HttpClient? _client;
        public static HttpClient GetClient()
        {
            _client ??= CreateClient();
            return _client;
        }

        private static HttpClient CreateClient()
        {
            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2)
            };
            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(GetConfig().GetRequiredSection("BaseUrl").Value)
            };
            return client;
        }

        private static IConfiguration GetConfig() => new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
    }
}