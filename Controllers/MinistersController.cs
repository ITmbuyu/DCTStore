using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DCTStore.Data;
using DCTStore.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office2010.Excel;
using DCTStore.ViewModels;
using DCTStore.Data.Migrations;

namespace DCTStore.Controllers
{
    public class MinistersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment; // Add this field

        public MinistersController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

		// GET: Ministers
		public async Task<IActionResult> Index(int page = 1, int pageSize = 9)
		{
			var totalItems = await _context.Ministers.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var paginatedMinisters = await _context.Ministers
				.Include(m => m.MininsterType)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;

			return View(paginatedMinisters);
		}


		// Get Local Ministers
		public async Task<IActionResult> LocalMinistersIndex(int page = 1, int pageSize = 9)
		{
			var sermonTypesWithCounts = await _context.Sermons
				.GroupBy(m => m.MediaType)
				.Select(group => new { MediaType = group.Key, Count = group.Count() })
				.ToListAsync();

			var latestUploads = await _context.Sermons
				.OrderByDescending(m => m.DatePreached)
				.Take(3)
				.ToListAsync();

			//add a viewbag that will hold the ministers
			ViewBag.Ministers = await _context.Ministers
				.Where(m => m.MininsterTypeId == 1)
				.OrderBy(x => Guid.NewGuid())
				.Take(10)
				.ToListAsync();

			var localministers = _context.Ministers
				.Include(m => m.MininsterType)
				.Where(s => s.MinisterId != 7 && s.MininsterTypeId == 1);

			var totalItems = await localministers.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var paginatedMinisters = await localministers
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;
            ViewBag.SermonTypes = sermonTypesWithCounts;
            ViewBag.RecentSermons = latestUploads;
            ViewData["Title"] = "Local Ministers"; // Set the title for the view

			return View(paginatedMinisters);
		}


		// Get Visiting Ministers except for Brother Ewarld Frank
		public async Task<IActionResult> VisitoringMinistersIndex(int page = 1, int pageSize = 9)
		{
            var sermonTypesWithCounts = await _context.Sermons
                .GroupBy(m => m.MediaType)
                .Select(group => new { MediaType = group.Key, Count = group.Count() })
                .ToListAsync();

            var latestUploads = await _context.Sermons
                .OrderByDescending(m => m.DatePreached)
                .Take(3)
                .ToListAsync();

            //add a viewbag that will hold the ministers
            ViewBag.Ministers = await _context.Ministers
                .Where(m => m.MininsterTypeId == 2)
                .OrderBy(x => Guid.NewGuid())
                .Take(10)
                .ToListAsync();

            var visitingministers = _context.Ministers
				.Include(m => m.MininsterType)
				.Where(m => m.MininsterTypeId == 2 && m.Name != "Brother Ewarld Frank");

			var totalItems = await visitingministers.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var paginatedMinisters = await visitingministers
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;
            ViewBag.SermonTypes = sermonTypesWithCounts;
            ViewBag.RecentSermons = latestUploads;
            ViewData["Title"] = " DCT Visiting Ministers"; // Set the title for the view

            return View(paginatedMinisters);
		}


		// Get: Sermons for a specific Minister
		public async Task<IActionResult> SermonsForMinister(int? ministerId, int page = 1, int pageSize = 8)
		{
            var sermonTypesWithCounts = await _context.Sermons
                .GroupBy(m => m.MediaType)
                .Select(group => new { MediaType = group.Key, Count = group.Count() })
                .ToListAsync();

            var latestUploads = await _context.Sermons
                .OrderByDescending(m => m.DatePreached)
                .Take(3)
                .ToListAsync();

            //add a viewbag that will hold the ministers
            ViewBag.Ministers = await _context.Ministers
                .Where(m => m.MininsterTypeId == 2)
                .OrderBy(x => Guid.NewGuid())
                .Take(10)
                .ToListAsync();

            var minister = await _context.Ministers
				.FirstOrDefaultAsync(m => m.MinisterId == ministerId);

			if (minister == null)
			{
				return NotFound();
			}

			var sermons = _context.Sermons
				.Include(s => s.MediaType)
				.Where(s => s.MinisterId == ministerId);

			var totalItems = await sermons.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var paginatedSermons = await sermons
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;
            ViewBag.SermonTypes = sermonTypesWithCounts;
            ViewBag.RecentSermons = latestUploads;
            ViewData["Title"] = " DCT Sermons"; // Set the title for the view

            return View(paginatedSermons);
		}


		// Get: Sermon Type for a specific Minister
		public async Task<IActionResult> MinisterIndexTopics(int? MinisterId, int page = 1, int pageSize = 9)
        {
            if (MinisterId == null || _context.Sermons == null)
            {
                return NotFound();
            }

			var sermonTypesWithCounts = await _context.Sermons
	.GroupBy(m => m.MediaType)
	.Select(group => new { MediaType = group.Key, Count = group.Count() })
	.ToListAsync();

			var latestUploads = await _context.Sermons
				.OrderByDescending(m => m.DatePreached)
				.Take(3)
				.ToListAsync();

			//add a viewbag that will hold the ministers
			ViewBag.Ministers = await _context.Ministers
				.Where(m => m.MininsterTypeId == 1)
				.OrderBy(x => Guid.NewGuid())
				.Take(10)
				.ToListAsync();

			

			var recentSermonsTopics = _context.Sermons
				.Where(s => s.Minister.MinisterId == MinisterId)
				.GroupBy(s => s.SermonTypeId)
				.Select(g => new SermonSubjects
				{
					SermonTypeId = g.Key.Value,
					SermonType = g.First().SermonType.Type,
					MediaType = g.First().MediaType,
					DatePreached = g.First().DatePreached,
					MinisterName = g.First().Minister.Name
				})
				.OrderByDescending(s => s.DatePreached);

			var totalItems = await recentSermonsTopics.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var paginatedTopics = await recentSermonsTopics
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;
			ViewBag.SermonTypes = sermonTypesWithCounts;
			ViewBag.RecentSermons = latestUploads;
			ViewData["Title"] = " DCT Sermon Topics"; // Set the title for the view

			return View(paginatedTopics);
		}


		public async Task<IActionResult> GetSermonTopicByMinister(string mediaType, int page = 1, int pageSize = 9)
		{
			var sermonTypesWithCounts = await _context.Sermons
				.GroupBy(m => m.MediaType)
				.Select(group => new { MediaType = group.Key, Count = group.Count() })
				.ToListAsync();

			var latestUploads = await _context.Sermons
				.OrderByDescending(m => m.DatePreached)
				.Take(3)
				.ToListAsync();

			//add a viewbag that will hold the ministers
			ViewBag.Ministers = await _context.Ministers
				.Where(m => m.MininsterTypeId == 1)
				.OrderBy(x => Guid.NewGuid())
				.Take(10)
				.ToListAsync();

			//get the sermontype count by minister name of the current sermon use ministerid
			var sermonTypesWithCountsByMinister = await _context.Sermons
				.Where(s => s.Minister.Name == mediaType)
				.GroupBy(s => s.SermonType)
				.Select(group => new { SermonType = group.Key, Count = group.Count() })
				.ToListAsync();


			//pass the sermon type count by minister name to the view
			ViewBag.SermonTypesByMinister = sermonTypesWithCountsByMinister;

            ViewBag.SermonTypes = sermonTypesWithCounts;
			ViewBag.RecentSermons = latestUploads;

			var recentSermonsTopics = _context.Sermons
				.Where(s => s.Minister.Name == mediaType)
				.GroupBy(s => s.SermonTypeId)
				.Select(g => new SermonSubjects
				{
					SermonTypeId = g.Key.Value,
					SermonType = g.First().SermonType.Type,
					MediaType = g.First().MediaType,
					DatePreached = g.First().DatePreached,
					MinisterName = g.First().Minister.Name
				})
				.OrderByDescending(s => s.DatePreached);

			var totalItems = await recentSermonsTopics.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var paginatedTopics = await recentSermonsTopics
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			ViewBag.SermonTypes = sermonTypesWithCounts;
			ViewBag.RecentSermons = latestUploads;
			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;
			ViewData["Title"] = mediaType + " Sermon Topics"; // Set the title for the view

			return View(paginatedTopics);
		}


		// GET: Ministers/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ministers == null)
            {
                return NotFound();
            }

            var minister = await _context.Ministers
                .Include(m => m.MininsterType)
                .FirstOrDefaultAsync(m => m.MinisterId == id);
            if (minister == null)
            {
                return NotFound();
            }

            return View(minister);
        }

        // GET: Ministers/Create
        public IActionResult Create()
        {
            ViewData["MininsterTypeId"] = new SelectList(_context.MininsterTypes, "MininsterTypeId", "Type");
            return View();
        }

        // POST: Ministers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MinisterId,Name,Image,MininsterTypeId")] Minister minister, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                // Check if an image was uploaded
                if (Image != null && Image.Length > 0)
                {
                    // Generate a unique file name for the image (you can customize this logic)
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;

                    // Define the path to save the image in the wwwroot/uploads folder
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(uploadsFolder);

                    // Save the uploaded image to the file system
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    // Save the image file path to the database
                    minister.Image = "/uploads/" + uniqueFileName; // Relative path to the image
                    _context.Add(minister);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["MininsterTypeId"] = new SelectList(_context.MininsterTypes, "MininsterTypeId", "Type", minister.MininsterTypeId);
            return View(minister);
        }

        // GET: Ministers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ministers == null)
            {
                return NotFound();
            }

            var minister = await _context.Ministers.FindAsync(id);
            if (minister == null)
            {
                return NotFound();
            }
            ViewData["MininsterTypeId"] = new SelectList(_context.MininsterTypes, "MininsterTypeId", "Type", minister.MininsterTypeId);
            return View(minister);
        }

        // POST: Ministers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MinisterId,Name,Image,MininsterTypeId")] Minister minister, IFormFile Image)
        {
            if (id != minister.MinisterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check if an image was uploaded
                if (Image != null && Image.Length > 0)
                {
                    // Generate a unique file name for the image (you can customize this logic)
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;

                    // Define the path to save the image in the wwwroot/uploads folder
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(uploadsFolder);

                    // Save the uploaded image to the file system
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    // Save the image file path to the database
                    minister.Image = "/uploads/" + uniqueFileName; // Relative path to the image
                }

                try
                {
                    _context.Update(minister);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MinisterExists(minister.MinisterId))
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
            ViewData["MininsterTypeId"] = new SelectList(_context.MininsterTypes, "MininsterTypeId", "Type", minister.MininsterTypeId);
            // If the code reaches here, it means there was an issue with the form submission
            // or the image upload. Return to the edit view with validation errors.
            return View(minister);
        }

        // GET: Ministers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ministers == null)
            {
                return NotFound();
            }

            var minister = await _context.Ministers
                .Include(m => m.MininsterType)
                .FirstOrDefaultAsync(m => m.MinisterId == id);
            if (minister == null)
            {
                return NotFound();
            }

            return View(minister);
        }

        // POST: Ministers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ministers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Ministers'  is null.");
            }
            var minister = await _context.Ministers.FindAsync(id);
            if (minister != null)
            {
                _context.Ministers.Remove(minister);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MinisterExists(int id)
        {
            return (_context.Ministers?.Any(e => e.MinisterId == id)).GetValueOrDefault();
        }
    }
}
