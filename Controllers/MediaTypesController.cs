using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DCTStore.Data;
using DCTStore.Models;
using System.IO;
using Microsoft.AspNetCore.Http; // For IFormFile and HttpContextAccessor
using Microsoft.AspNetCore.Hosting; // For IWebHostEnvironment
using Microsoft.Extensions.Hosting.Internal;

namespace DCTStore.Controllers
{
    public class MediaTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment; // Add this field

        public MediaTypesController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment) // Inject IWebHostEnvironment here
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment; // Initialize _hostingEnvironment
        }

        // GET: MediaTypes
        public async Task<IActionResult> Index()
        {
              return _context.MediaTypes != null ? 
                          View(await _context.MediaTypes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.MediaTypes'  is null.");
        }

        // GET: MediaTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MediaTypes == null)
            {
                return NotFound();
            }

            var mediaType = await _context.MediaTypes
                .FirstOrDefaultAsync(m => m.MediaTypeId == id);
            if (mediaType == null)
            {
                return NotFound();
            }

            return View(mediaType);
        }

        // GET: MediaTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MediaTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MediaTypeId,Type")] MediaType mediaType, IFormFile Image)
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
                    mediaType.Image = "/uploads/" + uniqueFileName; // Relative path to the image

                    _context.Add(mediaType);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // If the code reaches here, it means there was an issue with the form submission
            // or the image upload. Return to the create view with validation errors.
            return View(mediaType);
        }




        // GET: MediaTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MediaTypes == null)
            {
                return NotFound();
            }

            var mediaType = await _context.MediaTypes.FindAsync(id);
            if (mediaType == null)
            {
                return NotFound();
            }
            return View(mediaType);
        }

        // POST: MediaTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MediaTypeId,Type,Image")] MediaType mediaType, IFormFile Image)
        {
            if (id != mediaType.MediaTypeId)
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
                    mediaType.Image = "/uploads/" + uniqueFileName; // Relative path to the image
                }

                try
                {
                    _context.Update(mediaType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MediaTypeExists(mediaType.MediaTypeId))
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
            // If the code reaches here, it means there was an issue with the form submission
            // or the image upload. Return to the edit view with validation errors.
            return View(mediaType);
        }

        // GET: MediaTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MediaTypes == null)
            {
                return NotFound();
            }

            var mediaType = await _context.MediaTypes
                .FirstOrDefaultAsync(m => m.MediaTypeId == id);
            if (mediaType == null)
            {
                return NotFound();
            }

            return View(mediaType);
        }

        // POST: MediaTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MediaTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.MediaTypes'  is null.");
            }
            var mediaType = await _context.MediaTypes.FindAsync(id);
            if (mediaType != null)
            {
                _context.MediaTypes.Remove(mediaType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MediaTypeExists(int id)
        {
          return (_context.MediaTypes?.Any(e => e.MediaTypeId == id)).GetValueOrDefault();
        }
    }
}
