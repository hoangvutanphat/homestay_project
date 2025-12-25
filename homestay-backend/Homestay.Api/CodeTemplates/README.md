# Hướng dẫn sử dụng Custom Scaffold Templates

## Các thay đổi đã thực hiện:

### 1. ✅ Đã cập nhật tất cả 10 Entity files
Thêm `private set` cho:
- **Id properties** - Không cho phép thay đổi primary key
- **Foreign keys** (UserId, RoomId, HostId, BookingId, HomestayId) - Bảo vệ quan hệ
- **CreatedAt timestamps** - Chỉ set một lần khi tạo
- **Navigation properties** (Host, Room, User, Reviews, Bookings, etc.) - EF Core quản lý

### 2. ✅ Đã tạo Custom Scaffold Templates

Đã tạo 2 template files tại `CodeTemplates/EFCore/`:
- **EntityType.t4** - Template cho entity classes
- **DbContext.t4** - Template cho DbContext class

## Cách sử dụng khi scaffold lại từ database:

```bash
# Xóa entity files cũ (nếu cần)
rm -rf Domain/Entities/*.cs

# Scaffold lại với custom template
dotnet ef dbcontext scaffold \
  "YourConnectionString" \
  Npgsql.EntityFrameworkCore.PostgreSQL \
  --output-dir Domain/Entities \
  --context-dir Infrastructure/Persistence \
  --context HomestayDbContext \
  --force
```

Template sẽ tự động:
- Thêm `private set` cho Id, foreign keys, và CreatedAt
- Thêm `private set` cho navigation properties
- Giữ `public set` cho các properties thông thường

## Lưu ý quan trọng:

1. **EF Core hỗ trợ private setters** - Không cần cấu hình thêm
2. **Không làm mất dữ liệu** - EF Core dùng reflection để set giá trị
3. **Tăng tính bảo mật** - Code bên ngoài không thể thay đổi trực tiếp

## Ví dụ sử dụng trong code:

```csharp
// ❌ KHÔNG THỂ - Compiler error
var user = new User { Id = Guid.NewGuid() };

// ✅ ĐÚNG - Phải dùng qua EF Core hoặc tạo methods
var user = new User();
dbContext.Users.Add(user); // EF Core tự set Id

// ❌ KHÔNG THỂ - Compiler error  
booking.UserId = someGuid;

// ✅ ĐÚNG - Tạo mới với đúng thông tin
var booking = new Booking();
// Set qua constructor hoặc method nếu cần
```

## Tạo methods để thay đổi dữ liệu (Recommended):

Tạo partial class file riêng (không bị ghi đè khi scaffold):

```csharp
// Domain/Entities/Homestay.Methods.cs
namespace Homestay.Api.Domain.Entities;

public partial class Homestay
{
    public static Homestay Create(Guid hostId, string name, string address, string city, string? description = null)
    {
        return new Homestay
        {
            Id = Guid.NewGuid(),
            HostId = hostId,
            Name = name,
            Address = address,
            City = city,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };
    }
    
    public void UpdateInfo(string name, string address, string city, string? description)
    {
        Name = name;
        Address = address;
        City = city;
        Description = description;
    }
}
```

Hoàn thành! ✨
