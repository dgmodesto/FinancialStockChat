
using FinancialChat.Web.Consumers;
using FinancialChat.Web.Consumers.Definitions;
using MassTransit;
using Microsoft.AspNetCore.Hosting;

namespace FinancialChat.Web.Configuration
{
    public static class MassTransitConfig
    {
        public static void AddMassTransitConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            //- Incluindo o serviço do MassTransit no contêiner  da ASP .NET Core
            //-Cria um novo service bus usando o RabbitMQ local e definindo a conexão, passando os parâmetros para o usuário e senha padrão
            //-Incluindo o serviço hosted do MassTransit que inicia e para de forma automática o serviço de bus
            services.AddMassTransit(bus =>
            {

                /*
                 MassTransit fully integrates with ASP.NET Core, including:
                    Microsoft Extensions Dependency Injection container configuration, including consumer, saga, and activity registration. The MassTransit interfaces are also registered:
                    IBusControl (singleton)
                    IBus (singleton)
                    ISendEndpointProvider (scoped)
                    IPublishEndpoint (scoped)
                    The MassTransitHostedService to automatically start and stop the bus
                    Health checks for the bus and receive endpoints
                    Configures the bus to use ILoggerFactory from the container
                 
                 */
                
                
                // Add a single consumer by type
                bus.AddConsumer(typeof(FinancialChatStockWebConsumer), typeof(FinancialChatStockWebConsumerDefinition));

                bus.UsingRabbitMq((ctx, busConfiguration) =>
                {
                    //busConfiguration.Host(configuration.GetConnectionString("RabbitMq"));
                    busConfiguration.Host(Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION"));
                    busConfiguration.ReceiveEndpoint(Environment.GetEnvironmentVariable("FINANCIAL_CHAT_STOCK_QUEUE_RESPONSE"), e =>
                    {
                        e.ConfigureConsumer<FinancialChatStockWebConsumer>(ctx);
                    });
                });
            });
            services.AddMassTransitHostedService();
        }

        public static void UseMassTransitConfiguration(this IApplicationBuilder app)
        {

        }
    }

}