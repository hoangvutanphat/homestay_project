using System;
using System.Collections.Generic;

namespace Homestay.Api.Domain.Entities;

public partial class Booking
{
    public Guid Id { get; private set; }

    public Guid UserId { get;  set; }

    public Guid RoomId { get;  set; }

    public DateOnly CheckIn { get; set; }

    public DateOnly CheckOut { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; private set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }
    
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Review> Reviews { get;  set; } = new List<Review>();

    public virtual Room Room { get;  set; } = null!;

    public virtual User User { get;  set; } = null!;
}
