using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdealShop.Data;

namespace IdealShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/orders/checkout
        [HttpGet("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var email = User.Identity?.Name;
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);

            if (customer == null) return NotFound();

            return Ok(new { Address = customer.Address });
        }

        // POST: api/orders/placeorder
        [HttpPost("placeorder")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest request)
        {
            var email = User.Identity?.Name;
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null) return Unauthorized();

            var cartItems = _context.CartItems.Where(c => c.CustomerId == customer.Id);
            _context.CartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return Ok($"✅ Order placed! Delivery to: {request.ShippingAddress}");
        }

        // DTO for order request
        public class OrderRequest
        {
            public string ShippingAddress { get; set; } = string.Empty;
        }
    }
}
