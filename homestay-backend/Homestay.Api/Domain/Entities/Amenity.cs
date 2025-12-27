using System;
using System.Collections.Generic;

namespace Homestay.Api.Domain.Entities;

public partial class Amenity
{
    public int Id { get; private set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Icon { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; private set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
