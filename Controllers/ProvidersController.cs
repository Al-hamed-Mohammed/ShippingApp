using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shipping_Label_App.Data;
using Shipping_Label_App.Models;

namespace Shipping_Label_App.Controllers
{
    public class ProvidersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProvidersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Providers
        public async Task<IActionResult> Index()
        {
            return View(await _context.providers.ToListAsync());
        }

        // GET: Providers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var providers = await _context.providers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (providers == null)
            {
                return NotFound();
            }

            return View(providers);
        }

        // GET: Providers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Providers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProviderName,ShipmentCost,DateCreated,DateModified")] Providers providers)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(providers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(providers);
        }

        // GET: Providers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var providers = await _context.providers.FindAsync(id);
            if (providers == null)
            {
                return NotFound();
            }
            return View(providers);
        }

        // POST: Providers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProviderName,ShipmentCost,DateCreated,DateModified")] Providers providers)
        {
            if (id != providers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(providers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProvidersExists(providers.Id))
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
            return View(providers);
        }

        // GET: Providers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var providers = await _context.providers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (providers == null)
            {
                return NotFound();
            }

            return View(providers);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var providers = await _context.providers.FindAsync(id);
            _context.providers.Remove(providers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProvidersExists(int id)
        {
            return _context.providers.Any(e => e.Id == id);
        }
    }
}
