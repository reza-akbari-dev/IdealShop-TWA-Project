using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdealShop.Data;
using Microsoft.EntityFrameworkCore;

namespace IdealShop.Controllers
{
    [Authorize(Roles = "Customer")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Orders/Checkout
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Checkout()
        {
            var email = User.Identity.Name;
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);

            if (customer == null) return NotFound();

            ViewBag.CustomerAddress = customer.Address;
            return View();
        }



        // POST: /Orders/PlaceOrder
        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(string shippingAddress)
        {
            var email = User.Identity?.Name;
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null) return Unauthorized();

            var cartItems = _context.CartItems.Where(c => c.CustomerId == customer.Id);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            TempData["OrderSuccess"] = $"Your order was placed successfully and will be delivered to the address associated with your account. {shippingAddress}";
            return RedirectToAction("OrderSuccess");
        }


        // GET: /Orders/OrderSuccess
        public IActionResult OrderSuccess()
        {
            return View();
        }


    }
}
