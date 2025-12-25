using System;
using System.Collections.Generic;

namespace Homestay.Api.Domain.Entities;

public partial class Room
{
    public Guid Id { get; private set; }

    public Guid HomestayId { get; private set; }

    public string RoomName { get; set; } = null!;

    public decimal BasePrice { get; set; }

    public int Capacity { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; private set; }

    public virtual ICollection<Booking> Bookings { get; private set; } = new List<Booking>();

    public virtual Homestay Homestay { get; private set; } = null!;

    public virtual ICollection<RoomAvailability> RoomAvailabilities { get; private set; } = new List<RoomAvailability>();

    public virtual ICollection<Amenity> Amenities { get; private set; } = new List<Amenity>();

    public virtual ICollection<Promotion> Promotions { get; private set; } = new List<Promotion>();
}
