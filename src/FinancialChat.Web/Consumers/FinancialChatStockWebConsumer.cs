using FinancialChat.Domain.Models;
using FinancialChat.Web.Data.Cache;
using FinancialChatBackend.Hubs;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Caching.Memory;

namespace FinancialChat.Web.Consumers
{
    public class FinancialChatStockWebConsumer : IConsumer<Message>
    {
        private readonly ILogger<FinancialChatStockWebConsumer> _logger;
        private readonly IHubContext<StockChatHub> _notificationHubContext;
        private readonly IMemoryCache _memoryCache;

        public FinancialChatStockWebConsumer(
            ILogger<FinancialChatStockWebConsumer> logger, IHubContext<StockChatHub> notificationHubContext, IMemoryCache memoryCache)
        {
            _logger = logger;
            _notificationHubContext = notificationHubContext;
            _memoryCache = memoryCache;
        }
        ///stock=aapl.us
        public Task Consume(ConsumeContext<Message> context)
        {
            _logger.LogInformation($"[{nameof(FinancialChatStockWebConsumer)}-{  nameof(Consume) }] : Consuming queue");
            var message = context.Message;

            string sender = Environment.GetEnvironmentVariable("BOT_USER_NAME") ?? "UNKOWN_USER";
            _logger.LogInformation($"[{nameof(FinancialChatStockWebConsumer)}-{  nameof(Consume) }] : try to return message for de sender about stock_code");
            _memoryCache.AddMessageToHistory(sender, message.UserNameSender, message.Content);
            return _notificationHubContext.Clients.Group(message.UserNameSender).SendAsync("ReceiveMessage", sender, message.Content);
        }
    }
}
