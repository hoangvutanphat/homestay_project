using System;
using System.Collections.Generic;

namespace Homestay.Api.Domain.Entities;

public partial class User
{
    public Guid Id { get; private set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string Role { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; private set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<Booking> Bookings { get;  set; } = new List<Booking>();

    public virtual ICollection<Homestay> Homestays { get;  set; } = new List<Homestay>();

    public virtual ICollection<Review> Reviews { get;  set; } = new List<Review>();
}
