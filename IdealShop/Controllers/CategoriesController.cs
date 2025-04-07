using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdealShop.Data;
using IdealShop.Models;

namespace IdealShop.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var category = await FindByIdAsync(id);
            return category == null ? NotFound() : View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (!ModelState.IsValid) return View(category);

            //   Ensure category name is unique
            if (await _context.Categories.AnyAsync(c => c.Name == category.Name))
            {
                ModelState.AddModelError("Name", "Category name already exists.");
                return View(category);
            }

            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var category = await FindByIdAsync(id);
            return category == null ? NotFound() : View(category);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id) return NotFound();
            if (!ModelState.IsValid) return View(category);

            try
            {
                //   Ensure category name is unique (excluding the current category)
                if (await _context.Categories.AnyAsync(c => c.Name == category.Name && c.Id != category.Id))
                {
                    ModelState.AddModelError("Name", "Category name already exists.");
                    return View(category);
                }

                _context.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Categories.AnyAsync(e => e.Id == category.Id)) return NotFound();
                throw;
            }
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var category = await FindByIdAsync(id);
            return category == null ? NotFound() : View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //   Helper Method to Find Category by ID
        private async Task<Category?> FindByIdAsync(int? id)
        {
            return id == null ? null : await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
