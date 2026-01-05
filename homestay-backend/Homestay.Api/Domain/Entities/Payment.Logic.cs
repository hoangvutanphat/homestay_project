namespace Homestay.Api.Domain.Entities;

public partial class Payment
{
    public Payment()
    {
        Id = Guid.NewGuid();
        Currency = "VND";
        Status = "PENDING";
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsSuccess(string? transactionId = null)
    {
        Status = "SUCCESS";
        PaidAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = "FAILED";
    }
}
