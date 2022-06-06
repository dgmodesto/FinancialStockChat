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

            if (IsValidMessage(message))
            {
                var stock = await _stooqIntegrationService.GetStockByCodeAsync(message.Content);
                message.Content = stock;
                await _financialChatService.SendResponseStockByCode(message);

            }
            else
            {
                var messageError = $"sorry, the stock_code can't be empty, please, input a stock code and try again.";
                message.Content = messageError;
                await _financialChatService.SendResponseStockByCode(message);
            }
        }

        private bool IsValidMessage(Message message)
        {
            if (message == null) return false;
            else if (
                string.IsNullOrEmpty(message.Content) ||
                string.IsNullOrEmpty(message.UserNameSender) ||
                string.IsNullOrEmpty(message.UserNameReceive)
                )
                return false;

            return true;
        }
    }
}
