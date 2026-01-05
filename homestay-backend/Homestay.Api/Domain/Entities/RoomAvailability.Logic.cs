namespace Homestay.Api.Domain.Entities;

public partial class RoomAvailability
{
    public void Reserve(Guid bookingId, int expiryMinutes = 15)
    {
        IsAvailable = false;
        var expiryTime = DateTime.UtcNow.AddMinutes(expiryMinutes);
        Reason = $"RESERVED|{bookingId}|{expiryTime:O}";
    }

    public void Confirm(Guid bookingId)
    {
        IsAvailable = false;
        Reason = $"BOOKED|{bookingId}";
    }

    public void Release()
    {
        IsAvailable = true;
        Reason = null;
    }

    public bool IsReservedBy(Guid bookingId)
    {
        if (string.IsNullOrEmpty(Reason)) return false;
        var parts = Reason.Split('|');
        return parts.Length >= 2 && parts[1] == bookingId.ToString();
    }

    public bool IsExpired()
    {
        if (string.IsNullOrEmpty(Reason) || !Reason.StartsWith("RESERVED|")) return false;
        var parts = Reason.Split('|');
        if (parts.Length < 3) return false;
        if (DateTime.TryParse(parts[2], out var expiryTime))
        {
            return DateTime.UtcNow > expiryTime;
        }
        return false;
    }
}
