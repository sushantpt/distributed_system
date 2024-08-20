namespace payment.API.Dtos
{
    public class AccountDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Amount { get; set; }
        public string Email { get; init; } = string.Empty;
    }
}
