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
    public class LyricTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LyricTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LyricTypes
        public async Task<IActionResult> Index()
        {
              return _context.LyricTypes != null ? 
                          View(await _context.LyricTypes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.LyricTypes'  is null.");
        }

        // GET: LyricTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LyricTypes == null)
            {
                return NotFound();
            }

            var lyricType = await _context.LyricTypes
                .FirstOrDefaultAsync(m => m.LyricTypeId == id);
            if (lyricType == null)
            {
                return NotFound();
            }

            return View(lyricType);
        }

        // GET: LyricTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LyricTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LyricTypeId,Type")] LyricType lyricType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lyricType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lyricType);
        }

        // GET: LyricTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LyricTypes == null)
            {
                return NotFound();
            }

            var lyricType = await _context.LyricTypes.FindAsync(id);
            if (lyricType == null)
            {
                return NotFound();
            }
            return View(lyricType);
        }

        // POST: LyricTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LyricTypeId,Type")] LyricType lyricType)
        {
            if (id != lyricType.LyricTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lyricType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LyricTypeExists(lyricType.LyricTypeId))
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
            return View(lyricType);
        }

        // GET: LyricTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LyricTypes == null)
            {
                return NotFound();
            }

            var lyricType = await _context.LyricTypes
                .FirstOrDefaultAsync(m => m.LyricTypeId == id);
            if (lyricType == null)
            {
                return NotFound();
            }

            return View(lyricType);
        }

        // POST: LyricTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LyricTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LyricTypes'  is null.");
            }
            var lyricType = await _context.LyricTypes.FindAsync(id);
            if (lyricType != null)
            {
                _context.LyricTypes.Remove(lyricType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LyricTypeExists(int id)
        {
          return (_context.LyricTypes?.Any(e => e.LyricTypeId == id)).GetValueOrDefault();
        }
    }
}
