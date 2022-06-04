using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Integration.Interfaces
{
    public interface IStooqIntegrationService
    {
        Task<string> GetStockByCodeAsync(string stockCode);

    }
}
