using System;
using System.Collections.Generic;

namespace Homestay.Api.Domain.Entities;

public partial class Booking
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid RoomId { get; private set; }

    public DateOnly CheckIn { get; set; }

    public DateOnly CheckOut { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; private set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }
    
    public virtual ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    public virtual ICollection<Review> Reviews { get; private set; } = new List<Review>();

    public virtual Room Room { get; private set; } = null!;

    public virtual User User { get; private set; } = null!;
}
