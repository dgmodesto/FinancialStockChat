using FinancialChat.Application.Interfaces;
using FinancialChat.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplicationConfiguration(this IServiceCollection services)
        {
            services.AddServices();
        }


        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFinancialChatService, FinancialChatService>();
            return services;
        }
    }
}
