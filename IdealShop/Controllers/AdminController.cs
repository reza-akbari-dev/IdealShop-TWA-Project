using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdealShop.Data;
using IdealShop.Models;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    
    // LIST
    public async Task<IActionResult> Index()
    {
        return View(await _context.Admins.ToListAsync());
    }

 
    // DETAILS
    public async Task<IActionResult> Details(int? id)
    {
        var admin = await FindByIdAsync(id);
        return admin == null ? NotFound() : View(admin);
    }


    // CREATE (GET)
    public IActionResult Create()
    {
        return View();
    }

    // CREATE (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Admin admin)
    {
        if (!ModelState.IsValid)
            return View(admin);

        if (await _context.Admins.AnyAsync(a => a.Email == admin.Email))
        {
            ModelState.AddModelError("Email", "Email is already in use.");
            return View(admin);
        }

        (admin.Password, admin.Salt) = HashPassword(admin.Password);

        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

      
    // EDIT (GET)
    public async Task<IActionResult> Edit(int? id)
    {
        var admin = await FindByIdAsync(id);
        return admin == null ? NotFound() : View(admin);
    }

    // EDIT (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Admin admin)
    {
        if (id != admin.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(admin);

        var existingAdmin = await _context.Admins.FindAsync(id);
        if (existingAdmin == null)
            return NotFound();

        // Update fields
        existingAdmin.FirstName = admin.FirstName;
        existingAdmin.LastName = admin.LastName;
        existingAdmin.PhoneNumber = admin.PhoneNumber;
        existingAdmin.Address = admin.Address;

        // Update password if entered
        if (!string.IsNullOrWhiteSpace(admin.Password))
        {
            (existingAdmin.Password, existingAdmin.Salt) = HashPassword(admin.Password);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

      
    // DELETE (GET)
    public async Task<IActionResult> Delete(int? id)
    {
        var admin = await FindByIdAsync(id);
        return admin == null ? NotFound() : View(admin);
    }

    // DELETE (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var admin = await _context.Admins.FindAsync(id);
        if (admin != null)
        {
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

      
    // LOGIN (GET)
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    // LOGIN (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string email, string password)
    {
        var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
        if (admin == null || !VerifyPassword(password, admin.Password, admin.Salt))
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, admin.Email),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var identity = new ClaimsIdentity(claims, "MyCookieAuth");
        var principal = new ClaimsPrincipal(identity);
        var authProperties = new AuthenticationProperties();

        await HttpContext.SignInAsync("MyCookieAuth", principal, authProperties);
        return RedirectToAction(nameof(Index));
    }

      
    // LOGOUT
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookieAuth");
        return RedirectToAction(nameof(Login));
    }

      
    // HELPERS

    private async Task<Admin?> FindByIdAsync(int? id)
    {
        return id == null ? null : await _context.Admins.FirstOrDefaultAsync(a => a.Id == id);
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

    private bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
    {
        byte[] salt = Convert.FromBase64String(storedSalt);
        string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: enteredPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32
        ));

        return hash == storedHash;
    }
}
