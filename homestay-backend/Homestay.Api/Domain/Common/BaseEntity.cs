namespace Homestay.Api.Domain.Common;

/// <summary>
/// Base entity với soft delete support
/// Các entity khác có thể inherit class này
/// </summary>
public abstract class BaseEntity : ISoftDeletable
{
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
    
    /// <summary>
    /// Soft delete entity
    /// </summary>
    public virtual void SoftDelete(Guid? deletedBy = null)
    {
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
    
    /// <summary>
    /// Restore (undo soft delete)
    /// </summary>
    public virtual void Restore()
    {
        DeletedAt = null;
        DeletedBy = null;
    }
}
