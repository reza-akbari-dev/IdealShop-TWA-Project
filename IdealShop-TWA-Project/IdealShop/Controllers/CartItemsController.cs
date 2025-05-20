using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdealShop.Data;
using IdealShop.Models;

namespace IdealShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/cartitems
        [HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var cartItems = await _context.CartItems
        //        .Include(c => c.Customer)
        //        .Include(c => c.Product)
        //        .ToListAsync();

        //    return Ok(cartItems);
        //}
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cartItems = await _context.CartItems
                .Include(c => c.Customer)
                .Include(c => c.Product)
                .Where(c => c.Customer.Email == User.Identity.Name)
                .ToListAsync();

            return Ok(cartItems);
        }
        // GET: api/cartitems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cartItem = await _context.CartItems
                .Include(c => c.Customer)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.Id == id);

            return cartItem == null ? NotFound() : Ok(cartItem);
        }

        // POST: api/cartitems
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CartItem cartItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _context.Products.FindAsync(cartItem.ProductId);
            if (product == null)
                return BadRequest("Product does not exist.");

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.CustomerId == cartItem.CustomerId && c.ProductId == cartItem.ProductId);

            if (existingItem != null)
                return BadRequest("This product is already in the cart.");

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = cartItem.Id }, cartItem);
        }

        // PUT: api/cartitems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CartItem cartItem)
        {
            if (id != cartItem.Id)
                return BadRequest("ID mismatch.");

            _context.Entry(cartItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(cartItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.CartItems.AnyAsync(e => e.Id == id)) return NotFound();
                throw;
            }
        }

        // DELETE: api/cartitems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
                return NotFound();

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/cartitems/add
        [HttpPost("add")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddToCart([FromBody] CartAddRequest request)
        {
            Console.WriteLine("Current User: " + User.Identity.Name);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == User.Identity.Name);
            if (customer == null) return Unauthorized();

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.CustomerId == customer.Id && c.ProductId == request.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    CustomerId = customer.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok("Item added to cart.");
        }

        // POST: api/cartitems/placeorder
        [HttpPost("placeorder")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest request)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == User.Identity.Name);
            if (customer == null) return Unauthorized();

            var cartItems = _context.CartItems.Where(c => c.CustomerId == customer.Id);
            _context.CartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();
            return Ok("✅ Your order was placed successfully! It will be delivered to: " + request.ShippingAddress);
        }

        // Request DTOs
        public class CartAddRequest
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

        public class OrderRequest
        {
            public string ShippingAddress { get; set; } = string.Empty;
        }
    }
}
