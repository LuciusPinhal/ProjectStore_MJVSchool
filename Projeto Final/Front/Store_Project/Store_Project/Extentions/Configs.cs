using System.Net.Http;

namespace Store_Project.Extentions
{
    public static class Configs
    {

        public static void ConfigurarHttpClientStore(this IServiceCollection services)
        {
            services.AddHttpClient("Store", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7150");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}
