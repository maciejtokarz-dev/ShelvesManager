using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Models;
using System.Security.Claims;

namespace MyApp.Controllers
{
    [Authorize]
    public class ShelvesController : Controller
    {
        private readonly MyAppContext? _context;
        public ShelvesController(MyAppContext? context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var shelves = await _context.Shelves
                .Where(s => s.UserId == userId)
                .Include(s => s.Products)
                .ToListAsync();
            return View(shelves);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Shelf shelf)
        {
           shelf.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            ModelState.Remove("UserId");

            if (ModelState.IsValid) { 
                shelf.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

                _context.Shelves.Add(shelf);//dodanie do bazy
                await _context.SaveChangesAsync(); //zapisanie zmian
                return RedirectToAction(nameof(Index)); //wrocenie do listy
            }
            return View(shelf);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var shelf = await _context.Shelves
                .Include(s => s.Products) //zaladowanie produktow aby je usunac
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (shelf == null)
            {
                return NotFound();
            }
            _context.Shelves.Remove(shelf);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
