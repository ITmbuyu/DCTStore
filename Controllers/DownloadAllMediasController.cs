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
    public class DownloadAllMediasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DownloadAllMediasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DownloadAllMedias
        public async Task<IActionResult> Index()
        {
              return _context.DownloadAllMedia != null ? 
                          View(await _context.DownloadAllMedia.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.DownloadAllMedia'  is null.");
        }

        // GET: DownloadAllMedias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DownloadAllMedia == null)
            {
                return NotFound();
            }

            var downloadAllMedia = await _context.DownloadAllMedia
                .FirstOrDefaultAsync(m => m.DownloadAllMediaId == id);
            if (downloadAllMedia == null)
            {
                return NotFound();
            }

            return View(downloadAllMedia);
        }

        // GET: DownloadAllMedias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DownloadAllMedias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DownloadAllMediaId,Title,DownloadLink")] DownloadAllMedia downloadAllMedia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(downloadAllMedia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(downloadAllMedia);
        }

        // GET: DownloadAllMedias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DownloadAllMedia == null)
            {
                return NotFound();
            }

            var downloadAllMedia = await _context.DownloadAllMedia.FindAsync(id);
            if (downloadAllMedia == null)
            {
                return NotFound();
            }
            return View(downloadAllMedia);
        }

        // POST: DownloadAllMedias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DownloadAllMediaId,Title,DownloadLink")] DownloadAllMedia downloadAllMedia)
        {
            if (id != downloadAllMedia.DownloadAllMediaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(downloadAllMedia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DownloadAllMediaExists(downloadAllMedia.DownloadAllMediaId))
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
            return View(downloadAllMedia);
        }

        // GET: DownloadAllMedias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DownloadAllMedia == null)
            {
                return NotFound();
            }

            var downloadAllMedia = await _context.DownloadAllMedia
                .FirstOrDefaultAsync(m => m.DownloadAllMediaId == id);
            if (downloadAllMedia == null)
            {
                return NotFound();
            }

            return View(downloadAllMedia);
        }

        // POST: DownloadAllMedias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DownloadAllMedia == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DownloadAllMedia'  is null.");
            }
            var downloadAllMedia = await _context.DownloadAllMedia.FindAsync(id);
            if (downloadAllMedia != null)
            {
                _context.DownloadAllMedia.Remove(downloadAllMedia);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DownloadAllMediaExists(int id)
        {
          return (_context.DownloadAllMedia?.Any(e => e.DownloadAllMediaId == id)).GetValueOrDefault();
        }
    }
}
