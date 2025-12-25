using Homestay.Api.Domain.Common;
using Homestay.Api.Infrastructure.Extensions;
using Homestay.Api.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Homestay.Api.Infrastructure.Persistence;

/// <summary>
/// Extension của DbContext để configure Soft Delete
/// </summary>
public partial class HomestayDbContext
{
    /// <summary>
    /// Override SaveChanges để hỗ trợ soft delete
    /// </summary>
    public override int SaveChanges()
    {
        HandleSoftDelete();
        return base.SaveChanges();
    }

    /// <summary>
    /// Override SaveChangesAsync để hỗ trợ soft delete
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleSoftDelete();
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Configure soft delete trong OnModelCreating
    /// </summary>
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        // Apply soft delete query filters cho tất cả ISoftDeletable entities
        modelBuilder.ConfigureSoftDelete();
        
        // Configure soft delete columns cho các entities
        ConfigureSoftDeleteColumns(modelBuilder);
    }

    /// <summary>
    /// Configure columns cho soft delete
    /// </summary>
    private void ConfigureSoftDeleteColumns(ModelBuilder modelBuilder)
    {
        // User
        modelBuilder.Entity<Domain.Entities.User>(entity =>
        {
            entity.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at")
                .HasColumnType("timestamp without time zone");
            
            entity.Property(e => e.DeletedBy)
                .HasColumnName("deleted_by");
                
            entity.HasIndex(e => e.DeletedAt)
                .HasDatabaseName("ix_users_deleted_at");
        });

        // Homestay
        modelBuilder.Entity<HomestayEntity>(entity =>
        {
            entity.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at")
                .HasColumnType("timestamp without time zone");
            
            entity.Property(e => e.DeletedBy)
                .HasColumnName("deleted_by");
                
            entity.HasIndex(e => e.DeletedAt)
                .HasDatabaseName("ix_homestays_deleted_at");
        });

        // Booking
        modelBuilder.Entity<Domain.Entities.Booking>(entity =>
        {
            entity.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at")
                .HasColumnType("timestamp without time zone");
            
            entity.Property(e => e.DeletedBy)
                .HasColumnName("deleted_by");
                
            entity.HasIndex(e => e.DeletedAt)
                .HasDatabaseName("ix_bookings_deleted_at");
        });
    }

    /// <summary>
    /// Xử lý soft delete khi SaveChanges
    /// </summary>
    private void HandleSoftDelete()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted && e.Entity is ISoftDeletable);

        foreach (var entry in entries)
        {
            // Convert hard delete thành soft delete
            entry.State = EntityState.Modified;
            
            var entity = (ISoftDeletable)entry.Entity;
            entity.DeletedAt = DateTime.UtcNow;
            
            // TODO: Get current user ID from ICurrentUserService
            // entity.DeletedBy = _currentUserService.GetUserId();
        }
    }
}
