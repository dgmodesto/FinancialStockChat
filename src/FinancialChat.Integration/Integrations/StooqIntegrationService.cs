using FinancialChat.Integration.Interfaces;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Integration.Integrations
{
    internal class StooqIntegrationService : IStooqIntegrationService
    {
        public async Task<string> GetStockByCodeAsync(string stockCode)
        {
            var endponitStoq = $"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv";

            dynamic bytes = await endponitStoq.GetBytesAsync();
            var file = Encoding.UTF8.GetString(bytes);
            var values = file.Split('\n')[1].Split('\u002C');

            if (values[3] == "N/D")
                return $"sorry, I can't find the { stockCode}, please, verify if the stock code is correct";

            var response = $"{values[0]} quote is {values[3]} per share";

            return response;
        }
    }
}
