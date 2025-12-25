// using Homestay.Api.Domain.Entities;
// using Homestay.Api.Infrastructure.Persistence;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;

// namespace Homestay.Api.Controllers;

// [ApiController]
// [Route("api/users")]
// public class UserController : ControllerBase
// {
//     private readonly HomestayDbContext _context;

//     public UserController(HomestayDbContext context)
//     {
//         _context = context;
//     }

//     // Tạo user mới
//     [HttpPost]
//     public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
//     {
//         var user = new User
//         {
//             LastName = request.LastName,
//             Email = request.Email,
//             Phone = request.Phone,
//             Role = request.Role ?? "HOST" // Mặc định là HOST
//         };

//         _context.Users.Add(user);
//         await _context.SaveChangesAsync();

//         return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
//     }

//     // Lấy user theo ID
//     [HttpGet("{id}")]
//     public async Task<IActionResult> GetUser(Guid id)
//     {
//         var user = await _context.Users.FindAsync(id);
//         if (user == null)
//             return NotFound();

//         return Ok(user);
//     }

//     // Lấy tất cả users
//     [HttpGet]
//     public async Task<IActionResult> GetAllUsers()
//     {
//         var users = await _context.Users.ToListAsync();
//         return Ok(users);
//     }
// }

// // DTO để tạo user
// public class CreateUserRequest
// {
//     public string FullName { get; set; } = null!;
//     public string Email { get; set; } = null!;
//     public string? Phone { get; set; }
//     public string? Role { get; set; } // HOST, GUEST, ADMIN
// }
 