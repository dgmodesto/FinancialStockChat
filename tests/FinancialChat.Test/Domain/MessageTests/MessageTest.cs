using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinancialChat.Test.Domain.MessageTests
{
    [Collection(nameof(MessageCollection))]
    public class MessageTest
    {

        private readonly MessageFixture _fixture;

        public MessageTest(MessageFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact(DisplayName = "Message_Properties_IsValid")]
        public void RevenueKind_Properties_IsValid()
        {
            var model = _fixture.GenerateMessageModel();

            foreach (PropertyInfo pi in model.GetType().GetProperties())
            {
                Assert.NotNull(pi.GetValue(model));
            }
        }

    }
}
