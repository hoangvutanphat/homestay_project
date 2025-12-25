using Homestay.Api.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Homestay.Api.Infrastructure.Interceptors;

/// <summary>
/// Interceptor tự động convert hard delete thành soft delete
/// </summary>
public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
        {
            ConvertDeleteToSoftDelete(eventData.Context);
        }
        
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            ConvertDeleteToSoftDelete(eventData.Context);
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void ConvertDeleteToSoftDelete(DbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry is not { State: EntityState.Deleted, Entity: ISoftDeletable deletable })
                continue;

            // Convert hard delete thành soft delete
            entry.State = EntityState.Modified;
            deletable.DeletedAt = DateTime.UtcNow;
            
            // Có thể lấy current user ID từ IHttpContextAccessor hoặc ICurrentUserService
            // deletable.DeletedBy = _currentUserService.GetUserId();
        }
    }
}
