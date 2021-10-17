using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shipping_Label_App.Data;
using Shipping_Label_App.Models;

namespace Shipping_Label_App.Controllers
{
    [Authorize]
    public class ProvidersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProvidersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Providers
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.providers.ToListAsync());
        }

        // GET: Providers/Details/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Providers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProviderName,ShipmentCost,Maxweight,DateCreated,DateModified")] Providers providers)
        {
            if (ModelState.IsValid)
            {
                providers.DateCreated = DateTime.Now;
                _context.Add(providers);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(providers);
        }

        // GET: Providers/Edit/5
        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProviderName,ShipmentCost,Maxweight,DateCreated,DateModified")] Providers providers)
        {
            if (id != providers.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    providers.DateModified = DateTime.Now;
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
        [Authorize(Roles = "Admin")]
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
        public JsonResult GetMaxWeight(int id)
        {
            var prodviders  = _context.providers.Find(id);
            var maxweight = prodviders.Maxweight;
            return Json(new { message = maxweight.ToString() });
        }
        public JsonResult GetClassesByProviders(int id)
        {
            var classes = (from pc in _context.ProviderClasses
                            join p in _context.providers on pc.ProviderID equals p.Id
                            join c in _context.classes on pc.ClassID equals c.Id
                            where p.Id == id
                            select new
                            {
                                c.Id,
                                c.ClassName
                            }).ToList();

            return Json(new SelectList(classes, "Id", "ClassName"));
        }
    }
}
