using FinancialChat.Integration.Interfaces;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using System.Text;

namespace FinancialChat.Integration.Integrations
{
    public class StooqIntegrationService : IStooqIntegrationService
    {

        private readonly ILogger<StooqIntegrationService> _logger;

        public StooqIntegrationService(ILogger<StooqIntegrationService> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetStockByCodeAsync(string stockCode)
        {
            try
            {

                _logger.LogInformation($"[{nameof(StooqIntegrationService)}-{nameof(GetStockByCodeAsync)}] - Get the stock info at StookApi");

                var endponitStoq = $"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv";

                dynamic bytes = await endponitStoq.GetBytesAsync();

                var file = Encoding.UTF8.GetString(bytes);


                var values = file.Split('\n')[1].Split('\u002C');

                if (values[3] == "N/D")
                {
                    _logger.LogInformation($"[{nameof(StooqIntegrationService)}-{nameof(GetStockByCodeAsync)}] - the process don't find the {stockCode } at the stookApi");
                    return $"sorry, I can't find the { stockCode}, please, verify if the stock code is correct";
                }

                var stockCodeDescription = values[0];
                var valueFormat = "$" + values[3];
                var response = $"{ stockCodeDescription} quote is {valueFormat} per share";

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"[{nameof(StooqIntegrationService)}-{nameof(GetStockByCodeAsync)}] - Error - Description : { ex.Message }");
                return $"sorry, happened something, try again in some few minutes";
            }

        }
    }
}
