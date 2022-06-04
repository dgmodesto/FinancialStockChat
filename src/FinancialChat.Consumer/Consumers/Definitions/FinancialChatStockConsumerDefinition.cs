using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Consumer.Consumers.Definitions
{
    public  class FinancialChatStockConsumerDefinition : ConsumerDefinition<FinancialChatStockConsumer>
    {
        public FinancialChatStockConsumerDefinition()
        {
            EndpointName = Environment.GetEnvironmentVariable("FINANCIAL_CHAT_STOCK_QUEUE_RESPONSE");
            ConcurrentMessageLimit = 10;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<FinancialChatStockConsumer> consumerConfigurator)
        {
            // configure message retry with millisecond intervals
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

            // use the outbox to prevent duplicate events from being published
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}
