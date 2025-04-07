using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IdealShop.Data;
using IdealShop.Models;

namespace IdealShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = _context.Products.Include(p => p.ProductCategory);
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var product = await FindByIdAsync(id);
            return product == null ? NotFound() : View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            PopulateCategoriesDropdown();
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ProductCategoryId,Price,Stock,ImageFile")] Product product)
        {
            if (!ModelState.IsValid)
            {
                PopulateCategoriesDropdown(product.ProductCategoryId);
                return View(product);
            }

            //   Ensure product name is unique
            if (await _context.Products.AnyAsync(p => p.Name == product.Name))
            {
                ModelState.AddModelError("Name", "Product name already exists.");
                PopulateCategoriesDropdown(product.ProductCategoryId);
                return View(product);
            }

            //   Handle image upload
            if (product.ImageFile != null && product.ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.ImageFile.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(imagePath)!);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }

                product.ImageUrl = "/images/" + fileName;
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Please select an image.");
                PopulateCategoriesDropdown(product.ProductCategoryId);
                return View(product);
            }

            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var product = await FindByIdAsync(id);
            if (product == null) return NotFound();

            PopulateCategoriesDropdown(product.ProductCategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ProductCategoryId,Price,Stock,ImageUrl")] Product product)
        {
            if (id != product.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                PopulateCategoriesDropdown(product.ProductCategoryId);
                return View(product);
            }

            try
            {
                if (await _context.Products.AnyAsync(p => p.Name == product.Name && p.Id != product.Id))
                {
                    ModelState.AddModelError("Name", "Product name already exists.");
                    PopulateCategoriesDropdown(product.ProductCategoryId);
                    return View(product);
                }

                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Products.AnyAsync(e => e.Id == product.Id)) return NotFound();
                throw;
            }
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var product = await FindByIdAsync(id);
            return product == null ? NotFound() : View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //   Helper Method to Find Product by ID
        private async Task<Product?> FindByIdAsync(int? id)
        {
            return id == null ? null : await _context.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        //   Populate Categories in Dropdown
        private void PopulateCategoriesDropdown(object selectedCategory = null)
        {
            ViewData["ProductCategoryId"] = new SelectList(_context.Categories, "Id", "Name", selectedCategory);
        }
        // GET: Products/ByCategory/1
        public async Task<IActionResult> ByCategory(int categoryId)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
                return NotFound();

            ViewBag.CategoryName = category.Name;

            var products = await _context.Products
                .Include(p => p.ProductCategory)
                .Where(p => p.ProductCategoryId == categoryId)
                .ToListAsync();

            return View("Index", products); // Reuse Index view
        }

    }
}
