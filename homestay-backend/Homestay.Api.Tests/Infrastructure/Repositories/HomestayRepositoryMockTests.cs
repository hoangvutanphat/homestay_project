using Homestay.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace Homestay.Api.Tests.Infrastructure.Repositories;

/// <summary>
/// Unit tests cho HomestayRepository sử dụng Moq để mock DbContext
/// Approach này hữu ích khi bạn muốn kiểm soát hoàn toàn behavior của DbContext
/// Tuy nhiên, với Repository pattern đơn giản, In-Memory Database (test trước) thường tốt hơn
/// </summary>
public class HomestayRepositoryMockTests
{
    [Fact]
    public async Task GetAllHomestaysAsync_WithMock_ShouldCallDbSet()
    {
        // Arrange - Tạo mock data
        var mockHomestays = new List<Domain.Entities.Homestay>
        {
            // Note: Do Homestay có private setters, ta không thể mock dễ dàng
            // Đây là hạn chế của việc mock với entities có nhiều constraints
        };

        // Tạo options cho mock context
        var options = new DbContextOptionsBuilder<HomestayDbContext>()
            .UseInMemoryDatabase(databaseName: "MockTestDb")
            .Options;
        
        var mockContext = new Mock<HomestayDbContext>(options);
        
        // Setup mock DbSet
        mockContext.Setup(x => x.Homestays)
            .ReturnsDbSet(mockHomestays);

        var repository = new HomestayRepository(mockContext.Object);

        // Act
        var result = await repository.GetAllHomestaysAsync();

        // Assert
        Assert.NotNull(result);
        // Verify DbSet được gọi
        mockContext.Verify(x => x.Homestays, Times.Once);
    }

    [Fact]
    public async Task GetAllHomestaysAsync_WithRealContext_ShouldWork()
    {
        // Arrange - Với repository pattern, thường dùng real context trong tests
        var options = new DbContextOptionsBuilder<HomestayDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        var context = new HomestayDbContext(options);
        var repository = new HomestayRepository(context);

        // Act
        var result = await repository.GetAllHomestaysAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        
        // Cleanup
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
