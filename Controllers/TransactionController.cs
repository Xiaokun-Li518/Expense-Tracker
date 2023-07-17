using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace Expense_Tracker.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public TransactionController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return View(new List<Transaction>());
            }

            var applicationDbContext = _context.Transactions
                .Where(t => t.UserId == currentUser.Id)
                .Include(t => t.Category);

            return View(await applicationDbContext.ToListAsync());
        }



        // GET: Transaction/AddOrEdit
        public async Task<IActionResult> AddOrEdit(int id=0)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }
            await PopulateCategories();
            if (id == 0)
            {
                return View(new Transaction());
            }
            else
            {
                var transaction = await _context.Transactions.FindAsync(id);
                if (transaction == null || transaction.UserId != currentUser.Id)
                {
                    return Unauthorized();
                }
                return View (transaction);
            }
        }

        // POST: Transaction/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddOrEdit([Bind("TransactionId,CategoryId,Amount,Note,Date")] Transaction transaction)
        {
            await PopulateCategories();
            ModelState.Clear();
            var currentUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                Console.WriteLine(transaction.TransactionId);
                Console.WriteLine (transaction.Note);

                if (transaction.TransactionId == null)
                {
                    transaction.UserId = currentUser.Id;
                    _context.Add(transaction);
                }
                else
                {
                    var originalTransaction = await _context.Transactions.FindAsync(transaction.TransactionId);
                    if (originalTransaction == null || originalTransaction.UserId != currentUser.Id)
                    {
                        return Unauthorized();
                    }
                    // _context.Update(transaction);
                    originalTransaction.Date = transaction.Date;
                    originalTransaction.CategoryId = transaction.CategoryId;
                    originalTransaction.Amount = transaction.Amount;
                    originalTransaction.Note = transaction.Note;
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            var currentUser = await _userManager.GetUserAsync(User);
            if (transaction == null || transaction.UserId != currentUser.Id)
            {
                return Unauthorized();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [NonAction]
        public async Task PopulateCategories()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var CategoryCollection = _context.Categories
                .Where (c => c.UserId == currentUser.Id)
                .ToList();
            ViewBag.Categories = CategoryCollection;
        }
    }
}
