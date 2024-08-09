using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanzasPlus.Models;

namespace FinanzasPlus.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly FinancesContext _context;

        public TransactionsController(FinancesContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var financesContext = _context.Transactions.Include(t => t.Card).Include(t => t.PaymentTypeNavigation).Include(t => t.TransactionTypeNavigation).Include(t => t.User);
            return View(await financesContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Card)
                .Include(t => t.PaymentTypeNavigation)
                .Include(t => t.TransactionTypeNavigation)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "CardNumber");
            ViewData["PaymentType"] = new SelectList(_context.PaymentTypes, "Id", "Description");
            ViewData["TransactionType"] = new SelectList(_context.TransactionTypes, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TransactionType,PaymentType,CreationDate,CardId,Amount,Note")] Transaction transaction)
        {
            //if (ModelState.IsValid)
            {
                var card = await _context.Cards.FindAsync(transaction.CardId);
                card.Amount -= transaction.Amount;
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "Id", transaction.CardId);
            ViewData["PaymentType"] = new SelectList(_context.PaymentTypes, "Id", "Id", transaction.PaymentType);
            ViewData["TransactionType"] = new SelectList(_context.TransactionTypes, "Id", "Id", transaction.TransactionType);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", transaction.UserId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "CardNumber");
            ViewData["PaymentType"] = new SelectList(_context.PaymentTypes, "Id", "Description");
            ViewData["TransactionType"] = new SelectList(_context.TransactionTypes, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TransactionType,PaymentType,CreationDate,CardId,Amount,Note")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "Id", transaction.CardId);
            ViewData["PaymentType"] = new SelectList(_context.PaymentTypes, "Id", "Id", transaction.PaymentType);
            ViewData["TransactionType"] = new SelectList(_context.TransactionTypes, "Id", "Id", transaction.TransactionType);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", transaction.UserId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Card)
                .Include(t => t.PaymentTypeNavigation)
                .Include(t => t.TransactionTypeNavigation)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
