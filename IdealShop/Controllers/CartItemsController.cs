using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IdealShop.Data;
using IdealShop.Models;
using Microsoft.AspNetCore.Authorization;

namespace IdealShop.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CartItems
        public async Task<IActionResult> Index()
        {
            var cartItems = _context.CartItems
                .Include(c => c.Customer)
                .Include(c => c.Product);
            return View(await cartItems.ToListAsync());
        }

        // GET: CartItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var cartItem = await FindByIdAsync(id);
            return cartItem == null ? NotFound() : View(cartItem);
        }

        // GET: CartItems/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: CartItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerId,ProductId,Quantity")] CartItem cartItem)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns(cartItem.CustomerId, cartItem.ProductId);
                return View(cartItem);
            }

            //    Ensure product exists
            var product = await _context.Products.FindAsync(cartItem.ProductId);
            if (product == null)
            {
                ModelState.AddModelError("ProductId", "Selected product does not exist.");
                PopulateDropdowns(cartItem.CustomerId, cartItem.ProductId);
                return View(cartItem);
            }

            //    Prevent duplicate cart items (same customer, same product)
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.CustomerId == cartItem.CustomerId && c.ProductId == cartItem.ProductId);

            if (existingItem != null)
            {
                ModelState.AddModelError("", "This product is already in the cart.");
                PopulateDropdowns(cartItem.CustomerId, cartItem.ProductId);
                return View(cartItem);
            }

            _context.Add(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: CartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var cartItem = await FindByIdAsync(id);
            if (cartItem == null) return NotFound();

            PopulateDropdowns(cartItem.CustomerId, cartItem.ProductId);
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,ProductId,Quantity")] CartItem cartItem)
        {
            if (id != cartItem.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                PopulateDropdowns(cartItem.CustomerId, cartItem.ProductId);
                return View(cartItem);
            }

            try
            {
                _context.Update(cartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.CartItems.AnyAsync(e => e.Id == cartItem.Id)) return NotFound();
                throw;
            }
        }

        // GET: CartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var cartItem = await FindByIdAsync(id);
            return cartItem == null ? NotFound() : View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null) return NotFound();

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //    Helper Method to Find CartItem by ID
        private async Task<CartItem?> FindByIdAsync(int? id)
        {
            return id == null ? null : await _context.CartItems
                .Include(c => c.Customer)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        //    Populate Dropdowns for Customers & Products
        private void PopulateDropdowns(object selectedCustomer = null, object selectedProduct = null)
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Email", selectedCustomer);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", selectedProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Customer"))
                return RedirectToAction("Login", "Customers");

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == User.Identity.Name);
            if (customer == null) return NotFound();

            // Check if the product is already in cart
            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.CustomerId == customer.Id && c.ProductId == productId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    CustomerId = customer.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "CartItems");
        }
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(string shippingAddress)
        {
            // Get the logged-in customer's email
            var email = User.Identity.Name;

            // Find the customer
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null) return Unauthorized();

            // Find and remove all cart items for this customer
            var cartItems = _context.CartItems.Where(c => c.CustomerId == customer.Id);
            _context.CartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            TempData["OrderSuccess"] = "✅ Your order was placed successfully! It will be delivered to: " + shippingAddress;
            return RedirectToAction("OrderSuccess");
        }

    }
}
