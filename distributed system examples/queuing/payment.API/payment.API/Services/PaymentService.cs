using Contracts;
using MassTransit;
using Microsoft.Extensions.Caching.Memory;
using payment.API.Dtos;

namespace payment.API.Services
{
    public class PaymentService(IMemoryCache _cache, IPublishEndpoint _publishEndpoint) : IPaymentService
    {
        private readonly List<AccountDto> _accounts = new();

        public AccountDto GetAccountInfo(Guid accountId) 
            => _cache.TryGetValue("acc", out List<AccountDto>? accounts) ? accounts?.Find(x => x.Id.Equals(accountId))! : default!;

        public async Task<bool> TransferAmount(Guid fromAccountId, TransferDto transferDetail)
        {
            if (!_cache.TryGetValue("acc", out List<AccountDto>? allAccounts))
                return false;

            var sender = allAccounts!.Find(x => x.Id.Equals(fromAccountId));
            if (sender is null || sender.Amount < transferDetail.amount)
                return false;

            var receiver = allAccounts.Find(x => x.Id.Equals(transferDetail.account));
            if (receiver is null)
                return false;

            sender.Amount -= transferDetail.amount;
            receiver.Amount += transferDetail.amount;

            _cache.Set("acc", allAccounts);

            // publish message
            var promotionalReceiver = new List<PromotionalContract>
            {
                new (fromAccountId, sender.Email, "123456789", $"You account has been debited by {transferDetail.amount}. Transfered to {receiver.Name}."),
                new (transferDetail.account, receiver.Email, "123456454", $"You account has been credited by {transferDetail.amount}. Transfered by {sender.Name}.")
            };

            foreach (var item in promotionalReceiver)
            {
                await _publishEndpoint.Publish(item);
            }

            return true;
        }

        public bool SeedAccount()
        {
            for(int i = 0; i < 5; i++)
            {
                Random random = new();
                decimal amount = random.Next(50);

                _accounts.Add(new AccountDto()
                {
                    Id = Guid.NewGuid(),
                    Name = $"Ram{i}",
                    Email =$"ram{i}@mail.com",
                    Amount = amount
                });
            }
            _cache.Set("acc", _accounts);

            return true;
        }

        public List<AccountDto> GetAccounts() => _cache.TryGetValue("acc", out List<AccountDto>? accounts) ? accounts! : default!;
        
    }
}
