using FinancialChatBackend.Hubs;

namespace FinancialChat.Web.Configuration
{
    public static class SignalRConfig
    {

        public static void AddSignalRConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSignalR();

        }
        public static void UseSignalRConfiguration(this IApplicationBuilder app)
        {

        }
    }
}
