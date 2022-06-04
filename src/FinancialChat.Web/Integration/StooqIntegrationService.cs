using Flurl;
using Flurl.Http;
using System.Text;

namespace FinancialChatBackend.Integration
{
    public class StooqIntegrationService : IStooqIntegrationService
    {


        public async Task<string> GetStockByCode(string stockCode)
        {
            var endponitStoq = $"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv";

            dynamic bytes = await endponitStoq.GetBytesAsync();
            var file = Encoding.UTF8.GetString(bytes);
            var values = file.Split('\n')[1].Split('\u002C');

            var response = $"{values[0]} quote is {values[3]} per share";

            return response;
        }
    }
}
