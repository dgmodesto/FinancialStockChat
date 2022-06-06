using AutoMoq;
using Bogus;
using FinancialChat.Application.Interfaces;
using FinancialChat.Domain.Models;
using FinancialChatBackend.Hubs;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FinancialChat.Test.Web.HubTests
{
    [CollectionDefinition(nameof(StockChatHubCollection))]
    public class StockChatHubCollection : ICollectionFixture<StockChatHubFixture> { }

    public  class StockChatHubFixture
    {
        public Mock<IFinancialChatService> FinancialChatService;
        public Mock<IMemoryCache> MemoryCache;
        public Mock<Message> Message;


        public StockChatHub GenerateStockChatHub()
        {
            var mocker = new AutoMoqer();
            mocker.Create<StockChatHub>();
            var hub = mocker.Resolve<StockChatHub>();


            FinancialChatService = mocker.GetMock<IFinancialChatService>();
            MemoryCache = mocker.GetMock<IMemoryCache>();
            Message = mocker.GetMock<Message>();

            Mock<HubCallerContext> clientContext = new Mock<HubCallerContext>();
            Mock<IHubCallerClients> clients = new Mock<IHubCallerClients>();
            Mock<IGroupManager> groups = new Mock<IGroupManager>();
            Mock<IClientProxy> proxy = new Mock<IClientProxy>();

            var connectionId = Guid.NewGuid().ToString();
            clientContext.Setup(c => c.ConnectionId).Returns(connectionId);
            clientContext.Setup(c => c.User.Identity.Name).Returns("MyUser");
            clients.Setup(clients => clients.Caller).Returns(proxy.Object);
            
            clients.As<IHubClients>()
                .Setup(c => c.Client(connectionId))
                .Returns(proxy.Object);

            hub.Context = clientContext.Object;
            hub.Groups = groups.Object;
            hub.Clients = clients.Object;


            return hub;
        }

        public Message GenerateMessage()
        {
            var result = new Faker<Message>()
                .CustomInstantiator(f => new Message(
                   content: f.Random.String(),
                   userNameSender: f.Person.Email,
                   userNameReceive: f.Person.Email
                   ));

            return result;
        }


        public Queue<Message> GenerateQueueMessages(int quantity)
        {
            var queue = new Queue<Message>();
            var messages = new Faker<Message>()
                .CustomInstantiator(f => GenerateMessage())
                .Generate(quantity);
                
            foreach(Message message in messages)
                queue.Enqueue(message);

            return queue;
        }

    }

    public interface IClientContract
    {
        void broadcastMessage(string name, string message);
    }
}
