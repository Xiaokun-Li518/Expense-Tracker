using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace Expense_Tracker.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CategoryController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {

            var currentUser = await _userManager.GetUserAsync(User);

            var categories = await _context.Categories
                .Where(c => c.UserId == currentUser.Id)
                .Select(c => new Category 
                {
                    CategoryId = c.CategoryId,
                    UserId = c.UserId,
                    Title = c.Title,
                    Icon = c.Icon,
                    Type = c.Type
                })
                .ToListAsync();

            return View(categories);
        }


        // GET: Category/AddOrEdit
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (id == 0)
            {
                return View(new Category());
            }
            else
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null || category.UserId != currentUser.Id)
                {
                    return Unauthorized();
                }

                return View(category);
            }
        }

        // POST: Category/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("CategoryId,Title,Icon,Type")] Category category)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            
            ModelState.Clear();

            if (ModelState.IsValid)
            {
                if (category.CategoryId == 0)
                {
                    category.UserId = currentUser.Id;
                    _context.Add(category);
                }
                else
                {
                    var originalCategory = await _context.Categories.FindAsync(category.CategoryId);
                    if (originalCategory == null || originalCategory.UserId != currentUser.Id)
                    {
                        return Unauthorized();
                    }

                    // Update properties manually
                    originalCategory.Title = category.Title;
                    originalCategory.Icon = category.Icon;
                    originalCategory.Type = category.Type;
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }






        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var category = await _context.Categories.FindAsync(id);
            if (category == null || category.UserId != currentUser.Id)
            {
                return Unauthorized();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
