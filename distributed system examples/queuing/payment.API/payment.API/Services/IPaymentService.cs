using payment.API.Dtos;

namespace payment.API.Services
{
    public interface IPaymentService
    {
        AccountDto GetAccountInfo(Guid accountId);
        List<AccountDto> GetAccounts();
        bool SeedAccount();
        Task<bool> TransferAmount(Guid fromAccountId, TransferDto transferDetail);
    }
}
