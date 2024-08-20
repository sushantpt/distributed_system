using Contracts;
using MassTransit;

namespace promotional.SMS.Services
{
    public class SmsService(ILogger<PromotionalContract> _logger) : IConsumer<PromotionalContract>
    {
        public async Task Consume(ConsumeContext<PromotionalContract> context)
        {
            SendSMS(context.Message);
            await Task.CompletedTask;
        }

        private void SendSMS(PromotionalContract contract)
        {
            _logger.LogWarning("Sending Sms ... ");
            _logger.LogInformation("Sent Sms to {AccountId} with detail {Message}.", contract.AccountId, contract.Message);
        }
    }
}
