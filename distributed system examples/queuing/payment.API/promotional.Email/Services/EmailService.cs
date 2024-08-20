using Contracts;
using MassTransit;

namespace promotional.Email.Services
{
    public class EmailService : IConsumer<PromotionalContract>
    {
        public async Task Consume(ConsumeContext<PromotionalContract> context)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            SendEmail(context.Message);
            await Task.CompletedTask;
        }

        private void SendEmail(PromotionalContract detail)
        {
            Console.WriteLine("Sending email ...");
            Console.WriteLine($"Email sent to {detail.Email}, with message {detail.Message}");
        }
    }
}
