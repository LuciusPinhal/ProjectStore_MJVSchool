namespace FrontEnd.Extentions
{
    public static class ConfigInicial
    {
        public static int Duplicar(this int x)
        {
            return x * x;
        }

        public static void ConfigurarHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient("ClienteTempo", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7240");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
        public static void ConfigurarHttpClientAlunos(this IServiceCollection services)
        {
            services.AddHttpClient("Alunos", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7240");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

   
        
    }
}