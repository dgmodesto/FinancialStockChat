using FinancialChat.Integration.Integrations;
using FinancialChat.Integration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Integration.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddIntegrationConfiguration(this IServiceCollection services)
        {
            services.AddIntegrations();
        }


        private static IServiceCollection AddIntegrations(this IServiceCollection services)
        {
            services.AddScoped<IStooqIntegrationService, StooqIntegrationService>();
            return services;
        }
    }
}
