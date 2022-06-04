using FinancialChat.Application.Interfaces;
using FinancialChat.Domain.Models;
using FinancialChat.Integration.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FinancialChat.Consumer.Consumers
{
    public class FinancialChatStockConsumer : IConsumer<Message>
    {
        private readonly ILogger<FinancialChatStockConsumer> _logger;
        private readonly IStooqIntegrationService _stooqIntegrationService;
        private readonly IFinancialChatService _financialChatService;


        public FinancialChatStockConsumer(
            ILogger<FinancialChatStockConsumer> logger, 
            IStooqIntegrationService stooqIntegrationService, 
            IFinancialChatService financialChatService)
        {
            _logger = logger;
            _stooqIntegrationService = stooqIntegrationService;
            _financialChatService = financialChatService;
        }

        public async Task Consume(ConsumeContext<Message> context)
        {
            _logger.LogInformation($"[{nameof(FinancialChatStockConsumer)}-{  nameof(Consume) }] : Consuming queue");
            var message = context.Message;

            var stock = await _stooqIntegrationService.GetStockByCodeAsync(message.Content);
            
            var newMessage = new Message(stock, message.UserNameSender, message.UserNameReceive);

            await _financialChatService.SendResponseStockByCode(newMessage);
            
        }

    }
}
