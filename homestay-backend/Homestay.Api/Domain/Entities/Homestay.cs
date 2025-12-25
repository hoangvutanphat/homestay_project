using Homestay.Api.Domain.Common;

namespace Homestay.Api.Domain.Entities;

public partial class Homestay : ISoftDeletable
{
    public Guid Id { get; set; }

    public Guid HostId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual User Host { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
