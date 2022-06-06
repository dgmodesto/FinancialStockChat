using Bogus;
using FinancialChat.Web.Data.Cache;
using FinancialChatBackend.Hubs;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using System;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FinancialChat.Test.Web.HubTests
{
    [Collection(nameof(StockChatHubCollection))]
    public class StockChatHubTest
    {
        private readonly StockChatHubFixture _fixture;
        private readonly StockChatHub hub;


        public StockChatHubTest(StockChatHubFixture fixture)
        {
            _fixture = fixture;
            hub =_fixture.GenerateStockChatHub();
        }

        [Fact(DisplayName = "StockChatHub_OnConnectedAsync_ValidMessage")]
        public async Task StockChatHub_OnConnectedAsync_ValidMessage()
        {
            // arrange
            const int quantityHistoryMessage = 10;
            var historyMessage = _fixture.GenerateQueueMessages(quantityHistoryMessage);

            object objectOut = historyMessage;
            var cachEntry = Mock.Of<ICacheEntry>();
            _fixture.MemoryCache
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(cachEntry);


            _fixture.MemoryCache
                 .Setup(x => x.TryGetValue(It.IsAny<object>(), out objectOut))
                 .Returns(true);

            // act
            await hub.OnConnectedAsync();


            // assert
            Assert.NotEqual(null, hub.Groups);
            
        }

        [Fact(DisplayName = "FinancialChatStockConsumer_SendMessage_ValidMessage")]
        public async Task StockChatHub_SendMessage_ValidMessage()
        {
            //range 
            var mockClientProxy = new Mock<Microsoft.AspNetCore.SignalR.IClientProxy>();
            var mockClients = new Mock<IHubCallerClients>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            hub.Clients = mockClients.Object;
            var user = new Faker().Person.Email;
            var message = new Faker().Random.String();


            // act
            await hub.SendMessage(user, message);

            // assert
            mockClients.Verify(clients => clients.All, Times.Once);
        }

        [Fact(DisplayName = "StockChatHub_SendMessageToGroup_ValidMessage")]
        public async Task StockChatHub_SendMessageToGroup_ValidMessage()
        {
            //range 
            var sender = new Faker().Person.Email;
            var receiver = new Faker().Person.Email;
            var message = new Faker().Random.String();

            var mockClientProxy = new Mock<Microsoft.AspNetCore.SignalR.IClientProxy>();
            var mockClients = new Mock<IHubCallerClients>();
            mockClients.Setup(clients => clients.Group(receiver)).Returns(mockClientProxy.Object);
            hub.Clients = mockClients.Object;

            const int quantityHistoryMessage = 10;
            var historyMessage = _fixture.GenerateQueueMessages(quantityHistoryMessage);

            var cachEntry = Mock.Of<ICacheEntry>();
            _fixture.MemoryCache
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(cachEntry);


            // act
            await hub.SendMessageToGroup(sender, receiver,  message);

            // assert
            mockClients.Verify(clients => clients.Group(receiver), Times.Once);
        }

        [Fact(DisplayName = "StockChatHub_SendMessageToGroup_InValidMessage")]
        public async Task StockChatHub_SendMessageToGroup_InValidMessage()
        {
            //range 
            var sender = string.Empty;
            var receiver = string.Empty;
            var message = string.Empty;

            var mockClientProxy = new Mock<Microsoft.AspNetCore.SignalR.IClientProxy>();
            var mockClients = new Mock<IHubCallerClients>();
            mockClients.Setup(clients => clients.Group(receiver)).Returns(mockClientProxy.Object);
            hub.Clients = mockClients.Object;

            const int quantityHistoryMessage = 10;
            var historyMessage = _fixture.GenerateQueueMessages(quantityHistoryMessage);

            var cachEntry = Mock.Of<ICacheEntry>();
            _fixture.MemoryCache
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(cachEntry);

            Environment.SetEnvironmentVariable("BOT_USER_NAME", "bot");

            // act
            await hub.SendMessageToGroup(sender, receiver, message);

            // assert
            mockClients.Verify(clients => clients.Group(receiver), Times.Once);
        }

        [Fact(DisplayName = "StockChatHub_SendMessageToGroup_ValidMessageCommand")]
        public async Task StockChatHub_SendMessageToGroup_ValidMessageCommand()
        {
            //range 
            var sender = new Faker().Person.Email;
            var receiver = new Faker().Person.Email;
            var stock_code = "/stock=aapl.us";
            var message = _fixture.GenerateMessage();
            message.Content = stock_code;
            Environment.SetEnvironmentVariable("BOT_USER_NAME", "bot");

            _fixture.Message
                .SetReturnsDefault(message);

            var mockClientProxy = new Mock<Microsoft.AspNetCore.SignalR.IClientProxy>();
            var mockClients = new Mock<IHubCallerClients>();
            mockClients.Setup(clients => clients.Group(sender)).Returns(mockClientProxy.Object);
            hub.Clients = mockClients.Object;

            const int quantityHistoryMessage = 10;
            var historyMessage = _fixture.GenerateQueueMessages(quantityHistoryMessage);

            var cachEntry = Mock.Of<ICacheEntry>();
            _fixture.MemoryCache
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(cachEntry);

            _fixture.Message
                .SetReturnsDefault(message);


            // act
            await hub.SendMessageToGroup(sender, receiver, stock_code);

            // assert
            mockClients.Verify(clients => clients.Group(sender), Times.Once);

        }

        [Fact(DisplayName = "StockChatHub_SendMessageToGroup_InValidMessageCommand")]
        public async Task StockChatHub_SendMessageToGroup_InValidMessageCommand()
        {
            //range 
            var sender = new Faker().Person.Email;
            var receiver = new Faker().Person.Email;
            var stock_code = "/stock=";
            var message = _fixture.GenerateMessage();
            message.Content = stock_code;
            Environment.SetEnvironmentVariable("BOT_USER_NAME", "bot");

            _fixture.Message
                .SetReturnsDefault(message);

            var mockClientProxy = new Mock<Microsoft.AspNetCore.SignalR.IClientProxy>();
            var mockClients = new Mock<IHubCallerClients>();
            mockClients.Setup(clients => clients.Group(sender)).Returns(mockClientProxy.Object);
            hub.Clients = mockClients.Object;

            const int quantityHistoryMessage = 10;
            var historyMessage = _fixture.GenerateQueueMessages(quantityHistoryMessage);

            var cachEntry = Mock.Of<ICacheEntry>();
            _fixture.MemoryCache
                .Setup(m => m.CreateEntry(It.IsAny<object>()))
                .Returns(cachEntry);

            _fixture.Message
                .SetReturnsDefault(message);


            // act
            await hub.SendMessageToGroup(sender, receiver, stock_code);

            // assert
            mockClients.Verify(clients => clients.Group(sender), Times.Once);

        }
    }
}
