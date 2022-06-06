using AutoMoq;
using Bogus;
using FinancialChat.Application.Services;
using FinancialChat.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinancialChat.Test.Application.FinancialChatServiceTests
{
    [CollectionDefinition(nameof(FinancialChatServiceCollection))]
    public class FinancialChatServiceCollection : ICollectionFixture<FinancialChatServiceFixture> { }

    public class FinancialChatServiceFixture
    {

        public Mock<ILogger<FinancialChatService>> Logger;
        public Mock<IBus> Bus;
        public Mock<Message> Message;

        public FinancialChatService GetFinancialChatService()
        {
            var mocker = new AutoMoqer();
            mocker.Create<FinancialChatService>();
            var service = mocker.Resolve<FinancialChatService>();

            Logger = mocker.GetMock<ILogger<FinancialChatService>>();
            Bus = mocker.GetMock<IBus>();
            Message = mocker.GetMock<Message>();

            return service;
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
