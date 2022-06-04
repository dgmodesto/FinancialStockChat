using FinancialChat.Application.Interfaces;
using FinancialChat.Domain.Models;
using FinancialChatBackend.Integration;
using Microsoft.AspNetCore.SignalR;

namespace FinancialChatBackend.Hubs
{
    public class StockChatHub : Hub
    {

        public static Queue<Message> Messages;
        private readonly IStooqIntegrationService _stooqIntegrationService;
        private readonly IFinancialChatService _financialChatService;
        public StockChatHub(IStooqIntegrationService stooqIntegrationService, IFinancialChatService financialChatService)
        {

            if (Messages == null)
                Messages = new Queue<Message>();

            _stooqIntegrationService = stooqIntegrationService;
            _financialChatService = financialChatService;
        }


        public override Task OnConnectedAsync()
        {
            SendHistoryMessages();

            var userName = Context.User.Identity.Name == null ? "bot@financialchat.com" : Context.User.Identity.Name;

            Groups.AddToGroupAsync(Context.ConnectionId, userName);
            return base.OnConnectedAsync();
        }
        public async Task SendMessage(string user, string message)
        {
            //message send to all users
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public  Task SendMessageToGroup(string sender, string receiver, string message)
        {
            if(IsMessageComand(message))
            {
                var newReceiver = sender;
                var newSenderBot = "bot@financialchat.com";
                var stockCode = message.Split("=")[1];
                //var stock = _stooqIntegrationService.GetStockByCode(stockCode).Result;
                //var newMessage = stock;

                var requestMessage = new Message(stockCode, newSenderBot, newReceiver);
                _financialChatService.SendRequestStockByCode(requestMessage);

                string newMessage = "please, give me a second, I will get the information about the stock code for you.";
                AddMessageToHistory(newSenderBot, newReceiver, newMessage);
                return Clients.Group(newReceiver).SendAsync("ReceiveMessage", newSenderBot, newMessage);
            }

            //message send to receiver only
            AddMessageToHistory(sender, receiver, message);

            if (Clients == null)
                return Task.CompletedTask;

            return Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
        }

        private bool IsMessageComand(string message)
        {
            return (message.ToLower().Contains("/stock=")) ? true : false;
        }

        private void SendHistoryMessages()
        {
            foreach (var message in Messages)
            {
                Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", message.UserNameSender, message.Content);
            }

        }
        private void AddMessageToHistory(string userNameSender, string userNameReceive, string content)
        {
            var message = new Message(content, userNameSender, userNameReceive);
            const int HISTORY_MESSAGE_LIMIT = 50;
            
            if(Messages.Count == HISTORY_MESSAGE_LIMIT)
                Messages.Dequeue();

            Messages.Enqueue(message);
        }
    }
}
