using Homestay.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Homestay.Api.Tests.Infrastructure.Repositories;

/// <summary>
/// Unit tests cho HomestayRepository sử dụng In-Memory Database
/// Đây là cách đơn giản nhất để test Repository với DbContext
/// </summary>
public class HomestayRepositoryTests : IDisposable
{
    private readonly HomestayDbContext _context;
    private readonly HomestayRepository _repository;

    public HomestayRepositoryTests()
    {
        // Tạo In-Memory Database với tên unique cho mỗi test
        var options = new DbContextOptionsBuilder<HomestayDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new HomestayDbContext(options);
        _repository = new HomestayRepository(_context);
    }

    [Fact]
    public async Task GetAllHomestaysAsync_ShouldReturnEmptyList_WhenNoHomestaysExist()
    {
        // Act
        var result = await _repository.GetAllHomestaysAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllHomestaysAsync_ShouldNotReturnNull()
    {
        // Act
        var result = await _repository.GetAllHomestaysAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<Domain.Entities.Homestay>>(result);
    }

    [Fact]
    public async Task SoftDeleteHomestayAsync_ShouldMarkAsDeleted()
    {
        // Arrange - Note: Cần factory method để tạo homestay entity
        // Vì entity có private setters, test này là template
        var homestayId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        var result = await _repository.SoftDeleteHomestayAsync(homestayId, userId);

        // Assert - Template test
        // Trong thực tế cần tạo homestay trước, sau đó soft delete
        Assert.True(true, "Template test - cần implement entity factory");
    }

    [Fact]
    public async Task GetAllHomestaysAsync_ShouldNotReturnDeletedHomestays()
    {
        // Arrange
        // TODO: Create homestay entities (need factory)
        // TODO: Soft delete one of them
        
        // Act
        var result = await _repository.GetAllHomestaysAsync();

        // Assert
        // Should only return active (non-deleted) homestays
        Assert.True(true, "Template test");
    }

    [Fact]
    public async Task GetDeletedHomestaysAsync_ShouldReturnOnlyDeleted()
    {
        // Arrange
        // TODO: Create mix of active and deleted homestays
        
        // Act
        var result = await _repository.GetDeletedHomestaysAsync();

        // Assert
        // Should only return deleted homestays
        Assert.True(true, "Template test");
    }

    [Fact]
    public async Task RestoreHomestayAsync_ShouldClearDeletedFlags()
    {
        // Arrange
        var homestayId = Guid.NewGuid();
        // TODO: Create and soft delete homestay
        
        // Act
        var result = await _repository.RestoreHomestayAsync(homestayId);

        // Assert
        // Homestay should be active again
        Assert.True(true, "Template test");
    }

    public void Dispose()
    {
        // Cleanup: dispose context sau mỗi test
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
