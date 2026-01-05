using System;
using System.Collections.Generic;

namespace Homestay.Api.Domain.Entities;

public partial class Payment
{
    public Guid Id { get;  set; }

    public Guid BookingId { get;  set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? PaidAt { get; set; }

    public DateTime? CreatedAt { get; private set; }

    public virtual Booking Booking { get; private set; } = null!;
}
