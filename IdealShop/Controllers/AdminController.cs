using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdealShop.Data;
using IdealShop.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdealShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var admins = await _context.Admins.ToListAsync();
            return Ok(admins);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            return admin == null ? NotFound() : Ok(admin);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Admin admin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.Admins.AnyAsync(a => a.Email == admin.Email))
                return BadRequest("Email is already in use.");

            (admin.Password, admin.Salt) = HashPassword(admin.Password);

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = admin.Id }, admin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Admin admin)
        {
            if (id != admin.Id)
                return BadRequest("ID mismatch");

            var existing = await _context.Admins.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.FirstName = admin.FirstName;
            existing.LastName = admin.LastName;
            existing.PhoneNumber = admin.PhoneNumber;
            existing.Address = admin.Address;

            if (!string.IsNullOrWhiteSpace(admin.Password))
                (existing.Password, existing.Salt) = HashPassword(admin.Password);

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // ✅ DELETE: api/admin/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
                return NotFound();

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent(); // 204
        }

        // POST: api/admin/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == request.Email);
            if (admin == null || !VerifyPassword(request.Password, admin.Password, admin.Salt))
                return Unauthorized("Invalid email or password.");

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, admin.Email),
        new Claim(ClaimTypes.Role, "Admin")
    };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);
            var authProps = new AuthenticationProperties
            {
                IsPersistent = true // Optional: keep login after closing browser
            };

            await HttpContext.SignInAsync("MyCookieAuth", principal, authProps);
            return Ok("Login successful.");
        }

        // Helper to verify password
        private bool VerifyPassword(string password, string hash, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            string enteredHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32
            ));

            return enteredHash == hash;
        }

        // DTO for login
        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        // POST: api/admin/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return Ok("Logged out.");
        }
        private (string Hash, string Salt) HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32
            ));

            return (hash, Convert.ToBase64String(salt));
        }
    }
}
