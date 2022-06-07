using FinancialChat.Application.Interfaces;
using FinancialChat.Domain.Models;
using FinancialChat.Web.Data.Cache;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace FinancialChatBackend.Hubs
{
  public class StockChatHub : Hub
    {
        private readonly IFinancialChatService _financialChatService;
        private readonly IMemoryCache _memoryCache;
        private ILogger<StockChatHub> _logger;

        public StockChatHub(IFinancialChatService financialChatService, IMemoryCache memoryCache, ILogger<StockChatHub> logger)
        {
            _financialChatService = financialChatService;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name == null ? "bot@financialchat.com" : Context.User.Identity.Name;
            _logger.LogInformation($"[{nameof(StockChatHub)}-{nameof(OnConnectedAsync)}] - connecting user {userName}");
            SendHistoryMessages(userName);

            Groups.AddToGroupAsync(Context.ConnectionId, userName);
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            //message send to all users
            _logger.LogInformation($"[{nameof(StockChatHub)}-{nameof(SendMessage)}] - send message from user {user} to All");
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToGroup(string sender, string receiver, string message)
        {
            if (IsValidMessage(sender, receiver, message))
            {

                if (IsMessageComand(message))
                {
                    if (!IsValidMessageCommand(message))
                    {
                        message = $"sorry, the stock_code can't be empty, please, input a stock code and try again."; ;
                        return BotSendMessageToGroup(sender, message);
                    }

                    string receiverBot = GetBotUserName();
                    _memoryCache.AddMessageToHistory(sender, receiverBot, message);
                    var messageAr = message.Split("=");
                    var stockCode = messageAr[1];
                    var requestMessage = new Message(stockCode, sender, receiver);
                    
                    _financialChatService.SendRequestStockByCode(requestMessage);

                    message = "please, give me a second, I will get the information about the stock code to you.";
                    return BotSendMessageToGroup(sender, message);
                }

                //message send to receiver only
                 _memoryCache.AddMessageToHistory(sender, receiver, message);

                if (Clients == null)
                    return Task.CompletedTask;

                _logger.LogInformation($"[{nameof(StockChatHub)}-{nameof(SendMessageToGroup)}] -- send message from user {sender} to user { receiver }");
                return Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
            }
            else
            {
                _logger.LogInformation($"[{nameof(StockChatHub)}-{nameof(SendMessageToGroup)}] - invalid Message - Message:  { message } ");
                message = "Sorry, but I can't understand your message, try again please";
                return BotSendMessageToGroup(sender, message);
            }
        }

        private Task BotSendMessageToGroup(string receiver, string message)
        {
            string sender = GetBotUserName();

            //_memoryCache.AddMessageToHistory(sender, receiver, message);
            _logger.LogInformation($"[{nameof(StockChatHub)}-{nameof(SendMessageToGroup)}] - send message from user {sender} to user { receiver }");
            return Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
        }

        private string GetBotUserName() {
            return Environment.GetEnvironmentVariable("BOT_USER_NAME") ?? "UNKOWN_USER";
        }

        private void SendHistoryMessages(string key)
        {
            _logger.LogInformation($"[{nameof(StockChatHub)}-{nameof(SendHistoryMessages)}] - send history message to users");
            Queue<Message> historyMessages;
            if (_memoryCache.TryGetValue(key, out historyMessages))
            {
                foreach (var message in historyMessages)
                {
                    Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", message.UserNameSender, message.Content);
                }
            }

        }

        private bool IsValidMessage(string sender, string receiver, string message)
        {
            if (string.IsNullOrEmpty(sender) ||
                string.IsNullOrEmpty(receiver) ||
                string.IsNullOrEmpty(message)
                )
            {
                return false;
            }

            return true;
        }

        private bool IsMessageComand(string message)
        {
            return (message.ToLower().Contains("/stock=")) ? true : false;
        }

        private bool IsValidMessageCommand(string command)
        {
            var messageAr = command.Split("=");
            if (messageAr.Length < 2) return false;
            if (string.IsNullOrEmpty(messageAr[1])) return false;
            return true;
        }


    }
}
