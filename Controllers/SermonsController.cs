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
    public class SermonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SermonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sermons
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sermons.Include(s => s.MediaType).Include(s => s.Minister).Include(s => s.SermonType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sermons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sermons == null)
            {
                return NotFound();
            }

            // Select 4 random sermons from the database by sermon type of the current id
            var randomSermons = _context.Sermons
				.Where(s => s.SermonTypeId == id)
				.OrderBy(s => Guid.NewGuid())
				.Take(4)
				.ToList();

            // Put the random sermon in the SermonDetailsViewModel with the current sermon
            var viewModel = new SermonDetailsViewModel
            {
				CurrentSermon = await _context.Sermons
					.Include(s => s.MediaType)
					.Include(s => s.Minister)
					.Include(s => s.SermonType)
					.FirstOrDefaultAsync(s => s.SermonId == id),
				RelatedSermon = randomSermons
			};

            var sermon = await _context.Sermons
                .Include(s => s.MediaType)
                .Include(s => s.Minister)
                .Include(s => s.SermonType)
                .FirstOrDefaultAsync(m => m.SermonId == id);
            if (sermon == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: Sermons/Create
        public IActionResult Create()
        {
            ViewData["MediaTypeId"] = new SelectList(_context.MediaTypes, "MediaTypeId", "Type");
            ViewData["MinisterId"] = new SelectList(_context.Ministers, "MinisterId", "Name");
            ViewData["SermonTypeId"] = new SelectList(_context.SermonType, "SermonTypeId", "Type");
            return View();
        }

        // POST: Sermons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SermonId,Title,DatePreached,MinisterId,SermonMediaLink,SermonVideoLink,SermonTypeId,MediaTypeId")] Sermon sermon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sermon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MediaTypeId"] = new SelectList(_context.MediaTypes, "MediaTypeId", "Type", sermon.MediaTypeId);
            ViewData["MinisterId"] = new SelectList(_context.Ministers, "MinisterId", "Name", sermon.MinisterId);
            ViewData["SermonTypeId"] = new SelectList(_context.SermonType, "SermonTypeId", "Type", sermon.SermonTypeId);
            return View(sermon);
        }

        // GET: Sermons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sermons == null)
            {
                return NotFound();
            }

            var sermon = await _context.Sermons.FindAsync(id);
            if (sermon == null)
            {
                return NotFound();
            }
            ViewData["MediaTypeId"] = new SelectList(_context.MediaTypes, "MediaTypeId", "Type", sermon.MediaTypeId);
            ViewData["MinisterId"] = new SelectList(_context.Ministers, "MinisterId", "Name", sermon.MinisterId);
            ViewData["SermonTypeId"] = new SelectList(_context.SermonType, "SermonTypeId", "Type", sermon.SermonTypeId);
            return View(sermon);
        }

        // POST: Sermons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SermonId,Title,DatePreached,MinisterId,SermonMediaLink,SermonVideoLink,SermonTypeId,MediaTypeId")] Sermon sermon)
        {
            if (id != sermon.SermonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sermon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SermonExists(sermon.SermonId))
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
            ViewData["MediaTypeId"] = new SelectList(_context.MediaTypes, "MediaTypeId", "Type", sermon.MediaTypeId);
            ViewData["MinisterId"] = new SelectList(_context.Ministers, "MinisterId", "Name", sermon.MinisterId);
            ViewData["SermonTypeId"] = new SelectList(_context.SermonType, "SermonTypeId", "Type", sermon.SermonTypeId);
            return View(sermon);
        }

        // GET: Sermons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sermons == null)
            {
                return NotFound();
            }

            var sermon = await _context.Sermons
                .Include(s => s.MediaType)
                .Include(s => s.Minister)
                .Include(s => s.SermonType)
                .FirstOrDefaultAsync(m => m.SermonId == id);
            if (sermon == null)
            {
                return NotFound();
            }

            return View(sermon);
        }

        // POST: Sermons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sermons == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sermons'  is null.");
            }
            var sermon = await _context.Sermons.FindAsync(id);
            if (sermon != null)
            {
                _context.Sermons.Remove(sermon);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SermonExists(int id)
        {
          return (_context.Sermons?.Any(e => e.SermonId == id)).GetValueOrDefault();
        }
    }
}
