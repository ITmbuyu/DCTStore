using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DCTStore.Data;
using DCTStore.Models;
using DCTStore.ViewModels;

namespace DCTStore.Controllers
{
    public class LyricsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LyricsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lyrics
        public async Task<IActionResult> Index()
        {
              return _context.Lyrics != null ? 
                          View(await _context.Lyrics.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Lyrics'  is null.");
        }

        // GET: Lyrics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lyrics == null)
            {
                return NotFound();
            }

            // Select any 4 random lyric from the database 
            var randomLyrics = _context.Lyrics
                .OrderBy(m => Guid.NewGuid())
                .Take(4)
                .ToList();

            var lyric = await _context.Lyrics
                .FirstOrDefaultAsync(m => m.LyricId == id);
            if (lyric == null)
            {
                return NotFound();
            }

            // Put the random lyric in the LyricDetailsViewModel with the current lyric and also the thumbnail url of the random lyric
            var viewModel = new LyricDetailsViewModel
            {
                CurrentLyric = await _context.Lyrics
                    .FirstOrDefaultAsync(m => m.LyricId == id),
                RelatedLyric = randomLyrics,
                
            };

            return View(lyric);
        }

        // GET: Lyrics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lyrics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LyricId,Title,LyricMediaLink,LyricThumbnail")] Lyric lyric)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lyric);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lyric);
        }

        // GET: Lyrics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lyrics == null)
            {
                return NotFound();
            }

            var lyric = await _context.Lyrics.FindAsync(id);
            if (lyric == null)
            {
                return NotFound();
            }
            return View(lyric);
        }

        // POST: Lyrics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LyricId,Title,LyricMediaLink,LyricThumbnail")] Lyric lyric)
        {
            if (id != lyric.LyricId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lyric);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LyricExists(lyric.LyricId))
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
            return View(lyric);
        }

        // GET: Lyrics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lyrics == null)
            {
                return NotFound();
            }

            var lyric = await _context.Lyrics
                .FirstOrDefaultAsync(m => m.LyricId == id);
            if (lyric == null)
            {
                return NotFound();
            }

            return View(lyric);
        }

        // POST: Lyrics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lyrics == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Lyrics'  is null.");
            }
            var lyric = await _context.Lyrics.FindAsync(id);
            if (lyric != null)
            {
                _context.Lyrics.Remove(lyric);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LyricExists(int id)
        {
          return (_context.Lyrics?.Any(e => e.LyricId == id)).GetValueOrDefault();
        }
    }
}
