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
    public class MininsterTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MininsterTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MininsterTypes
        public async Task<IActionResult> Index()
        {
              return _context.MininsterTypes != null ? 
                          View(await _context.MininsterTypes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.MininsterTypes'  is null.");
        }

        // GET: MininsterTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MininsterTypes == null)
            {
                return NotFound();
            }

            var mininsterType = await _context.MininsterTypes
                .FirstOrDefaultAsync(m => m.MininsterTypeId == id);
            if (mininsterType == null)
            {
                return NotFound();
            }

            return View(mininsterType);
        }

        // GET: MininsterTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MininsterTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MininsterTypeId,Type")] MininsterType mininsterType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mininsterType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mininsterType);
        }

        // GET: MininsterTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MininsterTypes == null)
            {
                return NotFound();
            }

            var mininsterType = await _context.MininsterTypes.FindAsync(id);
            if (mininsterType == null)
            {
                return NotFound();
            }
            return View(mininsterType);
        }

        // POST: MininsterTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MininsterTypeId,Type")] MininsterType mininsterType)
        {
            if (id != mininsterType.MininsterTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mininsterType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MininsterTypeExists(mininsterType.MininsterTypeId))
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
            return View(mininsterType);
        }

        // GET: MininsterTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MininsterTypes == null)
            {
                return NotFound();
            }

            var mininsterType = await _context.MininsterTypes
                .FirstOrDefaultAsync(m => m.MininsterTypeId == id);
            if (mininsterType == null)
            {
                return NotFound();
            }

            return View(mininsterType);
        }

        // POST: MininsterTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MininsterTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MininsterTypes'  is null.");
            }
            var mininsterType = await _context.MininsterTypes.FindAsync(id);
            if (mininsterType != null)
            {
                _context.MininsterTypes.Remove(mininsterType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MininsterTypeExists(int id)
        {
          return (_context.MininsterTypes?.Any(e => e.MininsterTypeId == id)).GetValueOrDefault();
        }
    }
}
