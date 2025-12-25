namespace Homestay.Api.Domain.Common;

/// <summary>
/// Interface đánh dấu entity hỗ trợ soft delete
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Thời điểm entity bị xóa (soft delete)
    /// Null = chưa bị xóa
    /// </summary>
    DateTime? DeletedAt { get; set; }
    
    /// <summary>
    /// User ID của người thực hiện xóa
    /// </summary>
    Guid? DeletedBy { get; set; }
    
    /// <summary>
    /// Kiểm tra entity đã bị xóa chưa
    /// </summary>
    bool IsDeleted => DeletedAt.HasValue;
}
