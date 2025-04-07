using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdealShop.Data;
using IdealShop.Models;

namespace IdealShop.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        //  GET: All Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        //  GET: Customer Details
        public async Task<IActionResult> Details(int? id)
        {
            var customer = await FindByIdAsync(id);
            return customer == null ? NotFound() : View(customer);
        }

        //  GET: Create Customer
        public IActionResult Create()
        {
            return View();
        }

        //  POST: Create Customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Password,PhoneNumber,Address")] Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);

            if (await _context.Customers.AnyAsync(c => c.Email == customer.Email))
            {
                ModelState.AddModelError("Email", "Email is already registered.");
                return View(customer);
            }

            (customer.Password, customer.Salt) = HashPassword(customer.Password);

            _context.Add(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Edit Customer
        public async Task<IActionResult> Edit(int? id)
        {
            var customer = await FindByIdAsync(id);
            return customer == null ? NotFound() : View(customer);
        }

        //  POST: Edit Customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Password,PhoneNumber,Address")] Customer customer)
        {
            if (id != customer.Id) return NotFound();
            if (!ModelState.IsValid) return View(customer);

            var existing = await _context.Customers.FindAsync(id);
            if (existing == null) return NotFound();

            if (!string.IsNullOrEmpty(customer.Password))
            {
                (existing.Password, existing.Salt) = HashPassword(customer.Password);
            }

            existing.FirstName = customer.FirstName;
            existing.LastName = customer.LastName;
            existing.PhoneNumber = customer.PhoneNumber;
            existing.Address = customer.Address;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //  GET: Delete Customer
        public async Task<IActionResult> Delete(int? id)
        {
            var customer = await FindByIdAsync(id);
            return customer == null ? NotFound() : View(customer);
        }

        //  POST: Confirm Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        //  GET: Register
        public IActionResult Register()
        {
            return View();
        }

        //  POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);

            if (await _context.Customers.AnyAsync(c => c.Email == customer.Email))
            {
                ModelState.AddModelError("Email", "Email is already registered.");
                return View(customer);
            }

            (customer.Password, customer.Salt) = HashPassword(customer.Password);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        //  GET: Login
        public IActionResult Login()
        {
            return View();
        }

        //  POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null || !VerifyPassword(password, customer.Password, customer.Salt))
            {
                ModelState.AddModelError(string.Empty, "Invalid login credentials.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.Email),
                new Claim(ClaimTypes.Role, "Customer")
            };

            var identity = new ClaimsIdentity(claims, "CustomerLogin");
            var authProps = new AuthenticationProperties();

            await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(identity), authProps);

            return RedirectToAction("Index", "Home");
        }

        //  Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login");
        }

        //Find by ID
        private async Task<Customer?> FindByIdAsync(int? id)
        {
            return id == null ? null : await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        //  Hash
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

        //   Verify
        private bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32
            ));

            return hash == storedHash;
        }
    }
}
