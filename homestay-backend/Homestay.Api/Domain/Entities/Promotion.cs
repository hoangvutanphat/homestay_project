using System;
using System.Collections.Generic;

namespace Homestay.Api.Domain.Entities;

public partial class Promotion
{
    public Guid Id { get; private set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string DiscountType { get; set; } = null!;

    public decimal DiscountValue { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; private set; }

    public virtual ICollection<Room> Rooms { get; private set; } = new List<Room>();
}
