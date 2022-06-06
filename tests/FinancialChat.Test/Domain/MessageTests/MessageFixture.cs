using Bogus;
using FinancialChat.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinancialChat.Test.Domain.MessageTests
{
    [CollectionDefinition(nameof(MessageCollection))]
    public class MessageCollection : ICollectionFixture<MessageFixture> { }

    public class MessageFixture
    {

        public Message GenerateMessageModel()
        {
            return new Faker<Message>("pt_PT")
                .CustomInstantiator(f => new Message(f.Random.Word(), f.Person.Email, f.Person.Email));
        }

    }
}
