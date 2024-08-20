using Contracts;
using MassTransit;

namespace promotional.Email.Services
{
    public class EmailService(ILogger<PromotionalContract> _logger) : IConsumer<PromotionalContract>
    {
        public async Task Consume(ConsumeContext<PromotionalContract> context)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            SendEmail(context.Message);
            await Task.CompletedTask;
        }

        private void SendEmail(PromotionalContract detail)
        {
            _logger.LogWarning("Sending Email ... ");
            _logger.LogInformation("Email sent to {Email} with detail {Message}.", detail.Email, detail.Message);
        }
    }
}
