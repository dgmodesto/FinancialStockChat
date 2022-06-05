using FinancialChat.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace FinancialChat.Web.Data.Cache
{
    public static class HistoryChatCache
    {
        public static void AddMessageToHistory(this IMemoryCache _memoryCache, string userNameSender, string userNameReceive, string content)
        {
            SetCacheToSender(_memoryCache, userNameSender, userNameReceive, content);
            SetCacheToReceiver(_memoryCache, userNameSender, userNameReceive, content);
        }


        private static void SetCacheToSender(IMemoryCache _memoryCache, string userNameSender, string userNameReceive, string content)
        {
            var message = new Message(content, userNameSender, userNameReceive);
            const int HISTORY_MESSAGE_LIMIT = 50;

            Queue<Message> messagesAux;

            if (_memoryCache.TryGetValue(userNameSender, out messagesAux))
            {
                if (messagesAux.Count == HISTORY_MESSAGE_LIMIT)
                    messagesAux.Dequeue();

                messagesAux.Enqueue(message);
            }
            else
            {
                messagesAux = new Queue<Message>();
                messagesAux.Enqueue(message);
                _memoryCache.Set(userNameSender, messagesAux, TimeSpan.FromDays(7));
            }
        }


        private static void SetCacheToReceiver(IMemoryCache _memoryCache, string userNameSender, string userNameReceive, string content)
        {
            var message = new Message(content, userNameSender, userNameReceive);
            const int HISTORY_MESSAGE_LIMIT = 50;

            Queue<Message> messagesAux;

            if (_memoryCache.TryGetValue(userNameReceive, out messagesAux))
            {
                if (messagesAux.Count == HISTORY_MESSAGE_LIMIT)
                    messagesAux.Dequeue();

                messagesAux.Enqueue(message);

            }
            else
            {
                messagesAux = new Queue<Message>();
                messagesAux.Enqueue(message);
                _memoryCache.Set(userNameReceive, messagesAux, TimeSpan.FromDays(7));
            }
        }

    }
}
