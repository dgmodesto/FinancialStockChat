using AutoMoq;
using Bogus;
using FinancialChat.Application.Interfaces;
using FinancialChat.Consumer.Consumers;
using FinancialChat.Domain.Models;
using FinancialChat.Integration.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinancialChat.Test.Consumer.FinancialChatStockConsumerTests
{
    [CollectionDefinition(nameof(FinancialChatStockConsumerCollection))]
    public class FinancialChatStockConsumerCollection : ICollectionFixture<FinancialChatStockConsumerFixture> { }



    public class FinancialChatStockConsumerFixture
    {
        public Mock<ILogger<FinancialChatStockConsumer>> Logger;
        public Mock<IStooqIntegrationService> StooqIntegrationService;
        public Mock<IFinancialChatService> FinancialChatService;
        public Mock<Message> Message;

        public FinancialChatStockConsumer GenerateFinancialChatStockConsumer()
        {
            var mocker = new AutoMoqer();
            mocker.Create<FinancialChatStockConsumer>();
            var consumer = mocker.Resolve<FinancialChatStockConsumer>();

            Logger = mocker.GetMock<ILogger<FinancialChatStockConsumer>>();
            StooqIntegrationService = mocker.GetMock<IStooqIntegrationService>();
            FinancialChatService = mocker.GetMock<IFinancialChatService>();
            Message = mocker.GetMock<Message>();
            return consumer;
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

    }
}
