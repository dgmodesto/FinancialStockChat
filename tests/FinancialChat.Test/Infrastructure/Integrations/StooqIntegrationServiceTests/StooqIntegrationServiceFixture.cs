using AutoMoq;
using FinancialChat.Integration.Integrations;
using FinancialChat.Integration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FinancialChat.Test.Infrastructure.Integrations.StooqIntegrationServiceTests
{
    [CollectionDefinition(nameof(StooqIntegrationServiceCollection))]
    public class StooqIntegrationServiceCollection : ICollectionFixture<StooqIntegrationServiceFixture> { }

    public class StooqIntegrationServiceFixture
    {
        public StooqIntegrationService GetStooqIntegrationService()
        {
            var mocker = new AutoMoqer();
            mocker.Create<StooqIntegrationService>();
            var integration = mocker.Resolve<StooqIntegrationService>();

            return integration;
        }

    }
}
