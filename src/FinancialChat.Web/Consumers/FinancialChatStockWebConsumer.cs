using FinancialChat.Domain.Models;
using FinancialChatBackend.Hubs;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace FinancialChat.Web.Consumers
{
    public class FinancialChatStockWebConsumer : IConsumer<Message>
    {
        private readonly ILogger<FinancialChatStockWebConsumer> _logger;
        private readonly IHubContext<StockChatHub> _notificationHubContext;

        public FinancialChatStockWebConsumer(
            ILogger<FinancialChatStockWebConsumer> logger, IHubContext<StockChatHub> notificationHubContext)
        {
            _logger = logger;
            _notificationHubContext = notificationHubContext;
        }
        ///stock=aapl.us
        public async Task Consume(ConsumeContext<Message> context)
        {
            _logger.LogInformation($"[{nameof(FinancialChatStockWebConsumer)}-{  nameof(Consume) }] : Consuming queue");
            var message = context.Message;
            
            _logger.LogInformation($"[{nameof(FinancialChatStockWebConsumer)}-{  nameof(Consume) }] : try to return message for de sender about stock_code");
            
            await _notificationHubContext.Clients.Group(message.UserNameReceive).SendAsync("ReceiveMessage", message.UserNameSender, message.Content);
        }
    }
}
