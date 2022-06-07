using Bogus;
using FinancialChat.Integration.Integrations;
using Flurl.Http.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinancialChat.Test.Infrastructure.Integrations.StooqIntegrationServiceTests
{
    [Collection(nameof(StooqIntegrationServiceCollection))]
    public class StooqIntegrationServiceTest
    {
        private readonly StooqIntegrationServiceFixture _fixture;
        private readonly StooqIntegrationService _integration;

        public StooqIntegrationServiceTest(StooqIntegrationServiceFixture fixture)
        {
            _fixture = fixture;
            _integration = _fixture.GetStooqIntegrationService();
        }

        [Fact(DisplayName = "RevenueKindsService_GetStockByCodeAsync_ValidStockCode")]
        public async Task RevenueKindsService_GetStockByCodeAsync_ReturnStockInfo()
        {


            //Arrange
            using (var httpTest = new HttpTest())
            {
                var stock_code = new Faker().Random.Word();
                var response = "Symbol,Date,Time,Open,High,Low,Close,Volume \n AAPL.US,2022 - 06 - 06,22:00:10,147.03,148.5689,144.9,146.14,57364943";
                string expectedResult = " AAPL.US quote is $147.03 per share";
                httpTest.RespondWith(response);


                //Act
                string result = await _integration.GetStockByCodeAsync(stock_code);

                //Assert
                Assert.Equal(expectedResult, result);
            }

           
        }

        [Fact(DisplayName = "RevenueKindsService_GetStockByCodeAsync_InvalidStockCode")]
        public async Task RevenueKindsService_GetStockByCodeAsync_InvalidStockCode()
        {


            //Arrange
            using (var httpTest = new HttpTest())
            {
                var stock_code = new Faker().Random.Word();
                var response = "Symbol,Date,Time,Open,High,Low,Close,Volume\r\nAAPL,N/D,N/D,N/D,N/D,N/D,N/D,N/D\r\n";
                string expectedResult = "sorry, I can't find the";
                httpTest.RespondWith(response);


                //Act
                string result = await _integration.GetStockByCodeAsync(stock_code);

                //Assert
                Assert.Contains(expectedResult, result);
            }


        }

        [Fact(DisplayName = "RevenueKindsService_GetStockByCodeAsync_ThrowException")]
        public async Task RevenueKindsService_GetStockByCodeAsync_ThrowException()
        {


            //Arrange
            using (var httpTest = new HttpTest())
            {
                var stock_code = new Faker().Random.Word();
                var response = "";
                string expectedResult = "sorry, happened something, try again in some few minutes";
                httpTest.RespondWith(response);


                //Act
                string result = await _integration.GetStockByCodeAsync(stock_code);

                //Assert
                Assert.Contains(expectedResult, result);
            }


        }

    }
}
