using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

namespace FinancialChat.Web.Consumers.Definitions
{
    public class FinancialChatStockWebConsumerDefinition : ConsumerDefinition<FinancialChatStockWebConsumer>
    {
        public FinancialChatStockWebConsumerDefinition()
        {
            EndpointName = Environment.GetEnvironmentVariable("FINANCIAL_CHAT_STOCK_QUEUE_RESPONSE");
            ConcurrentMessageLimit = 10;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<FinancialChatStockWebConsumer> consumerConfigurator)
        {
            // configure message retry with millisecond intervals
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

            // use the outbox to prevent duplicate events from being published
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
