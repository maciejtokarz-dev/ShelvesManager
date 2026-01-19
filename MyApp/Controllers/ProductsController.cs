using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MyAppContext _context;
        public ProductsController(MyAppContext context)
        { _context = context; }


        public async Task<IActionResult> Index(int? shelfId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var shelves = await _context.Shelves
                .Where(s => s.UserId == userId)
                .ToListAsync();

            ViewBag.Shelves = shelves;
            
            if (shelfId.HasValue)
            {
                    var products = await _context.Products
                        .Include(p => p.Shelf)
                        .Where(p => p.ShelfId == shelfId.Value && p.Shelf!.UserId == userId)
                        .ToListAsync();

                ViewBag.CurrentShelfId = shelfId.Value;
                return View(products);
            }



            var allProducts = await _context.Products
                .Include(p => p.Shelf)
                .Where(p => p.Shelf!.UserId == userId)
                .ToListAsync();

            return View(allProducts);

        }

        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewData["Shelves"] = new SelectList(
                await _context.Shelves.Where(s => s.UserId == userId).ToListAsync(),
                "Id",
                "Name"
                );
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var shelf = await _context.Shelves
                .FirstOrDefaultAsync(s => s.Id == product.ShelfId && s.UserId == userId);

            if (shelf == null)
            {
                ModelState.AddModelError("ShelfId", "Nieprawidłowy regał");
                ViewData["Shelves"] = new SelectList(
                      await _context.Shelves.Where(s => s.UserId == userId)
                      .ToListAsync(), 
                      "Id", 
                      "Name"
                    );
                return View(product);
            }
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Shelves");
            }

            ViewData["Shelves"] = new SelectList(
                await _context.Shelves.Where(s => s.UserId == userId).ToListAsync(),
                "Id",
                "Name",
                product.ShelfId
                );
            return View(product);

            
        }
        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = await _context.Products
                .Include(p => p.Shelf)
                .FirstOrDefaultAsync(p => p.Id == id && p.Shelf!.UserId == userId);
            
            if(product == null)
            {
                return NotFound();
            }

            ViewData["Shelves"] = new SelectList(
                await _context.Shelves.Where(s => s.UserId == userId).ToListAsync(),
                    "Id",
                    "Name",
                    product.ShelfId
            );

            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if(id != product.Id)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var shelf = await _context.Shelves
                .FirstOrDefaultAsync(s => s.Id == product.ShelfId && s.UserId == userId);
            
            if(shelf == null) {
                
                ModelState.AddModelError("ShelfId", "Nieprawidłowy regał");

            }
            
            if (ModelState.IsValid)
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Shelves");
            }

            ViewData["Shelves"] = new SelectList(
                await _context.Shelves.Where(s => s.UserId == s.UserId)
                .ToListAsync(),
                "Id",
                "Name",
                product.ShelfId
                );
            return View(product);

        }
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var product = await _context.Products
                .Include(p => p.Shelf)
                .FirstOrDefaultAsync(p => p.Id == id && p.Shelf!.UserId == userId);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var product = await _context.Products
                .Include(p => p.Shelf)
                .FirstOrDefaultAsync(p => p.Id == id && p.Shelf!.User.Id == userId);
            
            if(product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();  
            }
            return RedirectToAction("Index", "Shelves");
        }
    }
}
