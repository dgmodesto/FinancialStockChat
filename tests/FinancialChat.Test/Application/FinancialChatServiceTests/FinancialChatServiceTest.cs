
using FinancialChat.Application.Services;
using FinancialChat.Domain.Models;
using MassTransit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinancialChat.Test.Application.FinancialChatServiceTests
{
    [Collection(nameof(FinancialChatServiceCollection))]
    public  class FinancialChatServiceTest
    {

        private readonly FinancialChatServiceFixture _fixture;
        private readonly FinancialChatService _service;

        public FinancialChatServiceTest(FinancialChatServiceFixture fixture)
        {
            _fixture = fixture;
            _service = _fixture.GetFinancialChatService();
        }

        [Fact(DisplayName = "FinancialChatService_SendRequestStockByCode_Success")]
        public async Task FinancialChatService_SendRequestStockByCode_Success()
        {

            //Arrange
            var message = _fixture.GenerateMessage();
            Environment.SetEnvironmentVariable("FINANCIAL_CHAT_STOCK_QUEUE_REQUEST", "queueName");

            _fixture.Message
                .SetReturnsDefault(message);

            var send = new Mock<ISendEndpoint>();

            _fixture.Bus
                .Setup(s => s.GetSendEndpoint(It.IsAny<Uri>()))
                .Returns(Task.FromResult(send.Object)) ;

            //Act
            await _service.SendRequestStockByCode(message);

            //Assert
            _fixture.Bus.Verify(s => s.GetSendEndpoint(It.IsAny<Uri>()), Times.Once);
        }

        [Fact(DisplayName = "FinancialChatService_SendRequestStockByCode_ThrowException")]
        public void FinancialChatService_SendRequestStockByCode_ThrowException()
        {

            //Arrange
            var message = _fixture.GenerateMessage();
            Environment.SetEnvironmentVariable("FINANCIAL_CHAT_STOCK_QUEUE_REQUEST", "queueName");

            _fixture.Message
                .SetReturnsDefault(message);

            var send = new Mock<ISendEndpoint>();

            //Act - Assert
            _ = Assert.ThrowsAsync<Exception>(async () => await _service.SendRequestStockByCode(message));

        }

        [Fact(DisplayName = "FinancialChatService_SendResponseStockByCode_Success")]
        public async Task FinancialChatService_SendResponseStockByCode_Success()
        {

            //Arrange
            var message = _fixture.GenerateMessage();
            Environment.SetEnvironmentVariable("FINANCIAL_CHAT_STOCK_QUEUE_RESPONSE", "queueName");

            _fixture.Message
                .SetReturnsDefault(message);

            var send = new Mock<ISendEndpoint>();

            _fixture.Bus
                .Setup(s => s.GetSendEndpoint(It.IsAny<Uri>()))
                .Returns(Task.FromResult(send.Object));

            //Act
            await _service.SendRequestStockByCode(message);

            //Assert
            _fixture.Bus.Verify(s => s.GetSendEndpoint(It.IsAny<Uri>()), Times.Once);
        }

    }
}
