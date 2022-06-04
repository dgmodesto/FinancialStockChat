namespace FinancialChatBackend.Integration
{
    public interface IStooqIntegrationService
    {
        Task<string> GetStockByCode(string stockCode);

    }
}
