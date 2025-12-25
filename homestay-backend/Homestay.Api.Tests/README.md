# Hướng Dẫn Unit Testing cho Classes có Dependency Injection

## Tổng Quan
Khi test các class có DI, có 2 phương pháp chính:

### 1. **Sử dụng In-Memory Database** (Khuyến nghị cho Repository)
Tạo database thật trong memory để test

**Ưu điểm:**
- Test gần với behavior thật nhất
- Không cần setup phức tạp
- Dễ hiểu và maintain

**Nhược điểm:**
- Chậm hơn mock thuần túy
- Cần cleanup sau mỗi test

**Ví dụ:**
```csharp
var options = new DbContextOptionsBuilder<HomestayDbContext>()
    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    .Options;

_context = new HomestayDbContext(options);
_repository = new HomestayRepository(_context);
```

### 2. **Sử dụng Moq Framework** (Khuyến nghị cho Services)
Mock các dependencies để kiểm soát behavior

**Ưu điểm:**
- Nhanh hơn
- Kiểm soát hoàn toàn behavior
- Isolate unit under test

**Nhược điểm:**
- Setup phức tạp hơn
- Có thể khác với behavior thật

**Ví dụ:**
```csharp
var mockRepository = new Mock<IHomestayRepository>();
mockRepository.Setup(x => x.GetAllHomestaysAsync())
    .ReturnsAsync(new List<HomestayEntity>());

_service = new HomestayService(mockRepository.Object);
```

## Cấu Trúc Test (AAA Pattern)

Mọi test nên theo pattern:

```csharp
[Fact]
public async Task MethodName_Condition_ExpectedBehavior()
{
    // Arrange - Chuẩn bị data và mock
    var data = CreateTestData();
    
    // Act - Thực thi method cần test
    var result = await _service.DoSomething(data);
    
    // Assert - Verify kết quả
    Assert.Equal(expectedValue, result);
}
```

## Các Loại Test Nên Viết

### 1. Happy Path Tests
Test các trường hợp thành công:
- `GetAll_ShouldReturnData_WhenDataExists`
- `Create_ShouldAddEntity_WhenValidData`

### 2. Edge Cases
Test các trường hợp đặc biệt:
- `GetAll_ShouldReturnEmpty_WhenNoData`
- `GetById_ShouldReturnNull_WhenNotFound`

### 3. Error Cases
Test các trường hợp lỗi:
- `Create_ShouldThrowException_WhenInvalidData`
- `Update_ShouldThrowException_WhenEntityNotFound`

## Chạy Tests

```bash
# Chạy tất cả tests
dotnet test

# Chạy tests với output chi tiết
dotnet test --logger "console;verbosity=detailed"

# Chạy tests và xem coverage
dotnet test /p:CollectCoverage=true

# Chạy test cụ thể
dotnet test --filter "FullyQualifiedName~HomestayRepositoryTests"
```

## Best Practices

### ✅ DO:
- Mỗi test chỉ test 1 behavior
- Tên test mô tả rõ ràng: `Method_Scenario_ExpectedResult`
- Sử dụng `Arrange-Act-Assert` pattern
- Cleanup resources (implement IDisposable nếu cần)
- Test cả success và error cases
- Mock external dependencies (API, Database, etc.)

### ❌ DON'T:
- Không test implementation details
- Không share state giữa các tests
- Không phụ thuộc vào execution order
- Không test framework code (EF Core, ASP.NET)
- Không mock mọi thứ (test behavior, không test mocks)

## Ví Dụ Test Service có nhiều Dependencies

```csharp
public class HomestayServiceTests
{
    private readonly Mock<IHomestayRepository> _mockRepository;
    private readonly Mock<ILogger<HomestayService>> _mockLogger;
    private readonly HomestayService _service;

    public HomestayServiceTests()
    {
        _mockRepository = new Mock<IHomestayRepository>();
        _mockLogger = new Mock<ILogger<HomestayService>>();
        _service = new HomestayService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetHomestays_ShouldCallRepository_Once()
    {
        // Arrange
        var expectedData = new List<HomestayEntity> { /* data */ };
        _mockRepository.Setup(x => x.GetAllHomestaysAsync())
            .ReturnsAsync(expectedData);

        // Act
        var result = await _service.GetHomestays();

        // Assert
        Assert.Equal(expectedData, result);
        _mockRepository.Verify(x => x.GetAllHomestaysAsync(), Times.Once);
    }
}
```

## Test Coverage Goals

- **Minimum:** 70% code coverage
- **Good:** 80% code coverage  
- **Excellent:** 90%+ code coverage

Nhưng nhớ: **Coverage cao không đồng nghĩa với test tốt!**

## Tài Nguyên Tham Khảo

- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq4)
- [EF Core Testing](https://learn.microsoft.com/en-us/ef/core/testing/)
- [Unit Testing Best Practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
