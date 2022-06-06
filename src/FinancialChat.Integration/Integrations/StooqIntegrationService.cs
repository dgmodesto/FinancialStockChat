using FinancialChat.Integration.Interfaces;
using Flurl.Http;
using System.Text;

namespace FinancialChat.Integration.Integrations
{
    public class StooqIntegrationService : IStooqIntegrationService
    {
        public async Task<string> GetStockByCodeAsync(string stockCode)
        {
            var endponitStoq = $"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv";

            dynamic bytes = await endponitStoq.GetBytesAsync();

            var file = Encoding.UTF8.GetString(bytes);
            
            
            var values = file.Split('\n')[1].Split('\u002C');

            if (values[3] == "N/D")
                return $"sorry, I can't find the { stockCode}, please, verify if the stock code is correct";

            var stockCodeDescription = values[0];
            var valueFormat = "$"+values[3]; 
            var response = $"{ stockCodeDescription} quote is {valueFormat} per share";

            return response;
        }
    }
}
