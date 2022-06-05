using FinancialChat.Application.Interfaces;
using FinancialChat.Domain.Models;
using FinancialChat.Web.Data.Cache;
using FinancialChatBackend.Integration;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Reflection;

namespace FinancialChatBackend.Hubs
{
    public class StockChatHub : Hub
    {
        private readonly IFinancialChatService _financialChatService;
        private readonly IMemoryCache _memoryCache;

        public StockChatHub(IFinancialChatService financialChatService, IMemoryCache memoryCache)
        {
            _financialChatService = financialChatService;
            _memoryCache = memoryCache;
        }

        public override Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name == null ? "bot@financialchat.com" : Context.User.Identity.Name;

            SendHistoryMessages(userName);

            Groups.AddToGroupAsync(Context.ConnectionId, userName);
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            //message send to all users
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToGroup(string sender, string receiver, string message)
        {
            if (IsValidMessage(sender, receiver, message))
            {
                _memoryCache.AddMessageToHistory(sender, sender, message);

                if (IsMessageComand(message))
                {
                    if (!IsValidMessageCommand(message))
                    {
                        message = $"sorry, the stock_code can't be empty, please, input a stock code and try again."; ;
                        return BotSendMessageToGroup(sender, message);
                    }

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

                return Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
            }
            else
            {
                message = "Sorry, but I can't understand your message, try again please";
                return BotSendMessageToGroup(sender, message);
            }
        }

        private Task BotSendMessageToGroup(string receiver, string message)
        {
            string sender = Environment.GetEnvironmentVariable("BOT_USER_NAME") ?? "UNKOWN_USER";

            _memoryCache.AddMessageToHistory(sender, receiver, message);
            return Clients.Group(receiver).SendAsync("ReceiveMessage", sender, message);
        }
        private void SendHistoryMessages(string key)
        {
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
