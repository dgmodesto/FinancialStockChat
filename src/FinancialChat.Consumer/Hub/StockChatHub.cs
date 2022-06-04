using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChatConsumer.Hub
{
    public  class StockChatHub
    {
        static HubConnection _connection;
        static string UserName;

        public async Task Connect()
        {
            Console.WriteLine("Iniciando conexão no signalR");


            var uri = "https://localhost:5077/chat";

            _connection = new HubConnectionBuilder()
                    .WithUrl(uri)
                    .Build();

            _connection.On<string>("newUser", (user) =>
            {
                var message = user == UserName ? "Voce entrou na sala" : $"{user } acabou de entrar";
                Console.WriteLine(message);
            });


            _connection.StartAsync();

            var newSenderBot = "bot@financialchat.com";
            //var stock = _stooqIntegrationService.GetStockByCode(stockCode).Result;
            var newMessage = "stock";

            await _connection.SendAsync("ReceiveMessage", newSenderBot, newMessage);

        }

    }
}
