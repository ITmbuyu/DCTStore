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
    public class MusicsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MusicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Musics
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Musics.Include(m => m.Lyric).Include(m => m.MediaType);
            return View(await applicationDbContext.ToListAsync());
        }

		// GET: Musics/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Musics == null)
			{
				return NotFound();
			}

			// Select 4 random music from the database by media type of the current id
			var randomMusic = _context.Musics
				.Where(m => m.MediaTypeId == id)
				.OrderBy(m => Guid.NewGuid())
				.Take(4)
				.ToList();

			// Put the random music in the MusicDetailsViewModel with the current music
			var viewModel = new MusicDetailsViewModel
			{
				CurrentMusic = await _context.Musics
					.Include(m => m.Lyric)
					.Include(m => m.MediaType)
					.FirstOrDefaultAsync(m => m.MusicId == id),
				RelatedMusic = randomMusic
			};



			// Return the view with the view model
			return View(viewModel);
		}


		// GET: Musics/Create
		public IActionResult Create()
        {
            ViewData["LyricId"] = new SelectList(_context.Lyrics, "LyricId", "LyricId");
            ViewData["MediaTypeId"] = new SelectList(_context.MediaTypes, "MediaTypeId", "MediaTypeId");
            return View();
        }

        // POST: Musics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MusicId,Title,MusicMediaLink,LyricId,MediaTypeId,DateUploaded")] Music music)
        {
            if (ModelState.IsValid)
            {
                _context.Add(music);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LyricId"] = new SelectList(_context.Lyrics, "LyricId", "LyricId", music.Title);
            ViewData["MediaTypeId"] = new SelectList(_context.MediaTypes, "MediaTypeId", "MediaTypeId", music.MediaTypeId);
            return View(music);
        }

        // GET: Musics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Musics == null)
            {
                return NotFound();
            }

            var music = await _context.Musics.FindAsync(id);
            if (music == null)
            {
                return NotFound();
            }
            ViewData["LyricId"] = new SelectList(_context.Lyrics, "LyricId", "LyricId", music.Title);
            ViewData["MediaTypeId"] = new SelectList(_context.MediaTypes, "MediaTypeId", "MediaTypeId", music.Title);
            return View(music);
        }

        // POST: Musics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MusicId,Title,MusicMediaLink,LyricId,MediaTypeId,DateUploaded")] Music music)
        {
            if (id != music.MusicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(music);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicExists(music.MusicId))
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
            ViewData["LyricId"] = new SelectList(_context.Lyrics, "LyricId", "LyricId", music.Title);
            ViewData["MediaTypeId"] = new SelectList(_context.MediaTypes, "MediaTypeId", "MediaTypeId", music.Title);
            return View(music);
        }

        // GET: Musics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Musics == null)
            {
                return NotFound();
            }

            var music = await _context.Musics
                .Include(m => m.Lyric)
                .Include(m => m.MediaType)
                .FirstOrDefaultAsync(m => m.MusicId == id);
            if (music == null)
            {
                return NotFound();
            }

            return View(music);
        }

        // POST: Musics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Musics == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Musics'  is null.");
            }
            var music = await _context.Musics.FindAsync(id);
            if (music != null)
            {
                _context.Musics.Remove(music);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MusicExists(int id)
        {
          return (_context.Musics?.Any(e => e.MusicId == id)).GetValueOrDefault();
        }
    }
}
