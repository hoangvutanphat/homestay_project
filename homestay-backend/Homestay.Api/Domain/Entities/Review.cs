using System;
using System.Collections.Generic;

namespace Homestay.Api.Domain.Entities;

public partial class Review
{
    public Guid Id { get; private set; }

    public Guid BookingId { get; private set; }

    public Guid UserId { get; private set; }

    public string ReviewerName { get; set; } = null!;

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; private set; }

    public Guid HomestayId { get; private set; }

    public virtual Booking Booking { get;  set; } = null!;

    public virtual Homestay Homestay { get;  set; } = null!;

    public virtual User User { get;  set; } = null!;
}
