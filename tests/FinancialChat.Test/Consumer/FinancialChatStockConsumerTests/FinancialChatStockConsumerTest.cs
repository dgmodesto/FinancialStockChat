using FinancialChat.Consumer.Consumers;
using FinancialChat.Domain.Models;
using MassTransit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinancialChat.Test.Consumer.FinancialChatStockConsumerTests
{
    [Collection(nameof(FinancialChatStockConsumerCollection))]
    public  class FinancialChatStockConsumerTest
    {

        private readonly FinancialChatStockConsumerFixture _fixture;
        private readonly FinancialChatStockConsumer _consumer;

        public FinancialChatStockConsumerTest(FinancialChatStockConsumerFixture fixture)
        {
            _fixture = fixture;
            _fixture.GenerateFinancialChatStockConsumer();
            _consumer = new FinancialChatStockConsumer(_fixture.Logger.Object, _fixture.StooqIntegrationService.Object, _fixture.FinancialChatService.Object);
        }


        [Fact(DisplayName = "FinancialChatStockConsumer_Consumer_ValidMessage")]
        public async Task FinancialChatStockConsumer_Consumer_ValidMessage()
        {
            //Arrange
            var message = _fixture.GenerateMessage();

            _fixture.Message
                .SetReturnsDefault(message);

            _fixture.FinancialChatService
                .Setup(x => x.SendResponseStockByCode(message));

            _fixture.StooqIntegrationService
                .Setup(x => x.GetStockByCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(message.Content);
                
            var context = Mock.Of<ConsumeContext<Message>>(_ =>
                 _.Message == message);


            //Act
            await _consumer.Consume(context);


            //Assert
            _fixture.StooqIntegrationService?.Verify(x => x.GetStockByCodeAsync(message.Content), Times.Once);
            _fixture.FinancialChatService?.Verify(x => x.SendResponseStockByCode(message), Times.Once);
            
        }

        [Fact(DisplayName = "FinancialChatStockConsumer_Consumer_InValidMessage")]
        public async Task FinancialChatStockConsumer_Consumer_InValidMessage()
        {
            //Arrange
            var message = _fixture.GenerateMessage();

   
            _fixture.FinancialChatService
                .Setup(x => x.SendResponseStockByCode(message));

            _fixture.StooqIntegrationService
                .Setup(x => x.GetStockByCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(message.Content);

            message = new Message();
            var context = Mock.Of<ConsumeContext<Message>>(_ =>
         _.Message == message);


            //Act
            await _consumer.Consume(context);


            //Assert
            _fixture.StooqIntegrationService?.Verify(x => x.GetStockByCodeAsync(message.Content), Times.Never);
            _fixture.FinancialChatService?.Verify(x => x.SendResponseStockByCode(message), Times.Once);

        }

    }
}
