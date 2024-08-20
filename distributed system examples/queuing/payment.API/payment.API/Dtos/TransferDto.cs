namespace payment.API.Dtos
{
    public record TransferDto(Guid account, decimal amount);
}
