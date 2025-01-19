using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DCTStore.Data;
using DCTStore.Models;

namespace DCTStore.Controllers
{
    public class SermonTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SermonTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SermonTypes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SermonType.Include(s => s.Minister);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SermonTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SermonType == null)
            {
                return NotFound();
            }

            var sermonType = await _context.SermonType
                .Include(s => s.Minister)
                .FirstOrDefaultAsync(m => m.SermonTypeId == id);
            if (sermonType == null)
            {
                return NotFound();
            }

            return View(sermonType);
        }

        // GET: SermonTypes/Create
        public IActionResult Create()
        {
            ViewData["MinisterId"] = new SelectList(_context.Ministers, "MinisterId", "MinisterId");
            return View();
        }

        // POST: SermonTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SermonTypeId,Type,MinisterId,TypeDownloadLink")] SermonType sermonType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sermonType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MinisterId"] = new SelectList(_context.Ministers, "MinisterId", "MinisterId", sermonType.MinisterId);
            return View(sermonType);
        }

        // GET: SermonTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SermonType == null)
            {
                return NotFound();
            }

            var sermonType = await _context.SermonType.FindAsync(id);
            if (sermonType == null)
            {
                return NotFound();
            }
            ViewData["MinisterId"] = new SelectList(_context.Ministers, "MinisterId", "MinisterId", sermonType.MinisterId);
            return View(sermonType);
        }

        // POST: SermonTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SermonTypeId,Type,MinisterId,TypeDownloadLink")] SermonType sermonType)
        {
            if (id != sermonType.SermonTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sermonType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SermonTypeExists(sermonType.SermonTypeId))
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
            ViewData["MinisterId"] = new SelectList(_context.Ministers, "MinisterId", "MinisterId", sermonType.MinisterId);
            return View(sermonType);
        }

        // GET: SermonTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SermonType == null)
            {
                return NotFound();
            }

            var sermonType = await _context.SermonType
                .Include(s => s.Minister)
                .FirstOrDefaultAsync(m => m.SermonTypeId == id);
            if (sermonType == null)
            {
                return NotFound();
            }

            return View(sermonType);
        }

        // POST: SermonTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SermonType == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SermonType'  is null.");
            }
            var sermonType = await _context.SermonType.FindAsync(id);
            if (sermonType != null)
            {
                _context.SermonType.Remove(sermonType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SermonTypeExists(int id)
        {
          return (_context.SermonType?.Any(e => e.SermonTypeId == id)).GetValueOrDefault();
        }
    }
}
