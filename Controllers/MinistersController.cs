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
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ministers.Include(m => m.MininsterType);
            return View(await applicationDbContext.ToListAsync());
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
