using System;
using System.Collections.Generic;

namespace Homestay.Api.Domain.Entities;

public partial class Image
{
    public Guid Id { get; private set; }

    public string EntityType { get; set; } = null!;

    public Guid EntityId { get; private set; } 

    public string ImageUrl { get; set; } = null!;

    public int? SortOrder { get; set; }

    public bool? IsCover { get; set; }

    public DateTime? CreatedAt { get; private set; }
}
