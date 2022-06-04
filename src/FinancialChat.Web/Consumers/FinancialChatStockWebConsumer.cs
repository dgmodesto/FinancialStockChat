using FinancialChat.Domain.Models;
using FinancialChatBackend.Hubs;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;

namespace FinancialChat.Web.Consumers
{
    public class FinancialChatStockWebConsumer : IConsumer<Message>
    {
        private readonly ILogger<FinancialChatStockWebConsumer> _logger;
        private HubConnection _connection;

        public FinancialChatStockWebConsumer(
            ILogger<FinancialChatStockWebConsumer> logger)
        {
            _logger = logger;

            var uri = "https://localhost:7077/chat";
            _connection = new HubConnectionBuilder()
                 .WithUrl(uri)
                 .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(10) })
                 .Build();
            
        }

        public async Task Consume(ConsumeContext<Message> context)
        {
            _logger.LogInformation($"[{nameof(FinancialChatStockWebConsumer)}-{  nameof(Consume) }] : Consuming queue");
            var message = context.Message;


            await StartIfNeededAsync();
            _connection.On<string>("ReceiveMessage", (user) =>
            {
            });
            await _connection.InvokeAsync("SendMessageToGroup", message.UserNameSender, message.UserNameReceive, message.Content);

        }

        public async Task StartIfNeededAsync()
        {
            if (_connection.State == HubConnectionState.Disconnected)
            {
                _connection.StartAsync();
            }
        }


     
    }
}
