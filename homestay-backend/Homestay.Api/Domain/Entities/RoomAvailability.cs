using System;
using System.Collections.Generic;

namespace Homestay.Api.Domain.Entities;

public partial class RoomAvailability
{
    public Guid Id { get; private set; }

    public Guid RoomId { get; private set; }

    public DateOnly Date { get; set; }

    public bool? IsAvailable { get; set; }

    public decimal? PriceOverride { get; set; }

    public string? Reason { get; set; }

    public DateTime? CreatedAt { get; private set; }

    public virtual Room Room { get; set; } = null!;
}
