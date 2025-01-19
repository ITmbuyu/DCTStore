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
using Azure.Storage.Blobs;

namespace DCTStore.Controllers
{
    public class LyricsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public LyricsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult ThankYouS(int? id)
        {
            ViewData["DownloadId"] = id; // Pass the ID to the view
            return View();
        }

        // GET: Lyrics
        public async Task<IActionResult> Index(int page = 1, int pageSize = 9)
        {
            var totalItems = await _context.Lyrics.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var musicTypesWithCounts = await _context.Musics
	.GroupBy(m => m.MediaType)
	.Select(group => new { MediaType = group.Key, Count = group.Count() })
	.ToListAsync();
			var lyricTypesWithCounts = await _context.Lyrics
                .GroupBy(m => m.LyricType)
                .Select(group => new { LyricType = group.Key, Count = group.Count() })
                .ToListAsync();

            var latestUploads = await _context.Lyrics.Take(3).ToListAsync();

            ViewBag.LyricTypes = lyricTypesWithCounts;
			ViewBag.MusicTypes = musicTypesWithCounts;
			ViewBag.LatestUploads = latestUploads;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            // Apply pagination here
            var paginatedLyrics = await _context.Lyrics
                .Include(l => l.LyricType)
               .Skip((page - 1) * pageSize)
.Take(pageSize)
.ToListAsync();

            return View(paginatedLyrics);
        }

        //GET: Worship Lyrics
        public async Task<IActionResult> WorshipLyrics(int page = 1, int pageSize = 9)
        {
            var totalItems = await _context.Lyrics.Include(l => l.LyricType)
 .Where(l => l.LyricType.Type == "DCT Worship Songs").CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var musicTypesWithCounts = await _context.Musics
.GroupBy(m => m.MediaType)
.Select(group => new { MediaType = group.Key, Count = group.Count() })
.ToListAsync();
			var lyricTypesWithCounts = await _context.Lyrics
                .GroupBy(m => m.LyricType)
                .Select(group => new { LyricType = group.Key, Count = group.Count() })
                .ToListAsync();

            var latestUploads = await _context.Lyrics.Take(3).ToListAsync();

            ViewBag.LyricTypes = lyricTypesWithCounts;
			ViewBag.MusicTypes = musicTypesWithCounts;
			ViewBag.LatestUploads = latestUploads;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            // Apply pagination here
            var paginatedLyrics = await _context.Lyrics
                .Include(l => l.LyricType)
                .Where(l => l.LyricType.Type == "DCT Worship Songs")
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(paginatedLyrics);
        }

        //GET: Praise Lyrics
        public async Task<IActionResult> PraiseLyrics(int page = 1, int pageSize = 9)
        {
            var totalItems = await _context.Lyrics.Include(l => l.LyricType)
.Where(l => l.LyricType.Type == "DCT Praise Songs").CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var musicTypesWithCounts = await _context.Musics
.GroupBy(m => m.MediaType)
.Select(group => new { MediaType = group.Key, Count = group.Count() })
.ToListAsync();
			var lyricTypesWithCounts = await _context.Lyrics
                .GroupBy(m => m.LyricType)
                .Select(group => new { LyricType = group.Key, Count = group.Count() })
                .ToListAsync();

            var latestUploads = await _context.Lyrics.Take(3).ToListAsync();

            ViewBag.LyricTypes = lyricTypesWithCounts;
			ViewBag.MusicTypes = musicTypesWithCounts;
			ViewBag.LatestUploads = latestUploads;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            // Apply pagination here
            var paginatedLyrics = await _context.Lyrics
                .Include(l => l.LyricType)
                .Where(l => l.LyricType.Type == "DCT Praise Songs")
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(paginatedLyrics);
        }

        //GET: DCT Choruses
        public async Task<IActionResult> ChorusesLyrics(int page = 1, int pageSize = 9)
        {
            var totalItems = await _context.Lyrics.Include(l => l.LyricType)
.Where(l => l.LyricType.Type == "DCT Choruses").CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var musicTypesWithCounts = await _context.Musics
.GroupBy(m => m.MediaType)
.Select(group => new { MediaType = group.Key, Count = group.Count() })
.ToListAsync();
			var lyricTypesWithCounts = await _context.Lyrics
                .GroupBy(m => m.LyricType)
                .Select(group => new { LyricType = group.Key, Count = group.Count() })
                .ToListAsync();

            var latestUploads = await _context.Lyrics.Take(3).ToListAsync();

            ViewBag.LyricTypes = lyricTypesWithCounts;
			ViewBag.MusicTypes = musicTypesWithCounts;
			ViewBag.LatestUploads = latestUploads;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            // Apply pagination here
            var paginatedLyrics = await _context.Lyrics
                .Include(l => l.LyricType)
                .Where(l => l.LyricType.Type == "DCT Choruses")
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(paginatedLyrics);
        }

        //GET: Special Songs
        public async Task<IActionResult> SpecialSongs(int page = 1, int pageSize = 9)
        {
            var totalItems = await _context.Lyrics.Include(l => l.LyricType)
.Where(l => l.LyricType.Type == "DCT Special Songs").CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var musicTypesWithCounts = await _context.Musics
.GroupBy(m => m.MediaType)
.Select(group => new { MediaType = group.Key, Count = group.Count() })
.ToListAsync();
			var lyricTypesWithCounts = await _context.Lyrics
                .GroupBy(m => m.LyricType)
                .Select(group => new { LyricType = group.Key, Count = group.Count() })
                .ToListAsync();

            var latestUploads = await _context.Lyrics.Take(3).ToListAsync();

            ViewBag.LyricTypes = lyricTypesWithCounts;
			ViewBag.MusicTypes = musicTypesWithCounts;
			ViewBag.LatestUploads = latestUploads;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            // Apply pagination here
            var paginatedLyrics = await _context.Lyrics
                .Include(l => l.LyricType)
                .Where(l => l.LyricType.Type == "DCT Special Songs")
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(paginatedLyrics);
        }

        public async Task<IActionResult> GetLyricsByMediaType(string mediaType, int page = 1, int pageSize = 9)
        {
            var lyricTypesWithCounts = await _context.Lyrics
                .GroupBy(m => m.LyricType)
                .Select(group => new { LyricType = group.Key, Count = group.Count() })
                .ToListAsync();

			var musicTypesWithCounts = await _context.Musics
.GroupBy(m => m.MediaType)
.Select(group => new { MediaType = group.Key, Count = group.Count() })
.ToListAsync();

			var latestUploads = await _context.Lyrics
                .OrderByDescending(m => m.LyricId)
                .Take(3)
                .ToListAsync();

            // Total number of records for the specified media type
            var totalItems = await _context.Lyrics
                .Where(m => m.LyricType.Type == mediaType)
                .CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Pagination: Skip records based on page number and take the next set of pageSize
            var applicationDbContext = await _context.Lyrics
                .Include(m => m.LyricType)
                .Where(m => m.LyricType.Type == mediaType)
                .OrderBy(m => m.LyricId) // Assuming you have an Id field to order by
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.LyricsTypes = lyricTypesWithCounts;
			ViewBag.MusicTypes = musicTypesWithCounts;
			ViewBag.LatestUploads = latestUploads;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewData["Title"] = mediaType; // Set the title for the view

            return View("GetLyricsByMediaType", applicationDbContext);
        }

        // GET: Lyrics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var lyricTypesWithCounts = await _context.Lyrics
    .GroupBy(m => m.LyricType)
    .Select(group => new { LyricType = group.Key, Count = group.Count() })
    .ToListAsync();
            //var latestUploads = await _context.Lyrics.Take(3).ToListAsync();

            ViewBag.LyricTypes = lyricTypesWithCounts;
            //ViewBag.LatestUploads = latestUploads;

            if (id == null || _context.Lyrics == null)
            {
                return NotFound();
            }

            var lyric = await _context.Lyrics
                .Include(l => l.LyricType)
                .FirstOrDefaultAsync(m => m.LyricId == id);
            if (lyric == null)
            {
                return NotFound();
            }

            // Select any 4 random lyric from the database
            var randomLyrics = _context.Lyrics
            .OrderBy(m => Guid.NewGuid())
            .Take(4)
            .ToList();



            //display latestuploads by latest record entry
            var latestUploads = await _context.Lyrics.Take(4).OrderByDescending(l => l.LyricId).ToListAsync();


            ViewBag.LatestUploads = latestUploads;

            if (id == null || _context.Musics == null)
            {
                return NotFound();
            }

            //GET MEDIATYPE ID OF CURRENT MUSIC
            var lyrictypeid = _context.Lyrics.Where(s => s.LyricId == id).Select(s => s.LyricTypeId).FirstOrDefault();




            //GET THE MEDIATYPE COUNT BY CURRENT MEDIATYPE USE MEDIATYPEID
            //          var mediatypeswithcountsbymediatype = await _context.Musics.Where(s => s.MediaTypeId == mediatypeid)
            //.GroupBy(s => s.MediaType)
            //.Select(group => new { MediaType = group.Key, Count = group.Count() })
            //.ToListAsync();
            var mediatypeswithcountsbymediatype = await _context.Lyrics.Where(m => m.LyricTypeId == lyrictypeid).Take(4).ToListAsync();

            ViewBag.lyricsbymediatype = mediatypeswithcountsbymediatype;

            // Generate 5 random Music IDs from the database
            var randomLyricIds = _context.Lyrics
                .Select(m => m.LyricId) // Select only the Music IDs
                .OrderBy(x => Guid.NewGuid()) // Randomize the order
                .Take(5) // Take 5 random IDs
                .ToList();

            // Select 4 random music records by matching with the randomMusicIds
            var randomMusic = _context.Lyrics
                .Where(m => randomLyricIds.Contains(m.LyricId)) // Filter by the random Music IDs
                .Include(m => m.LyricType)
                .Take(4) // Limit to 4 results
                .ToList();

            // Put the random lyric in the LyricDetailsViewModel with the current lyric and also the thumbnail url of the random lyric
            var viewModel = new LyricDetailsViewModel
            {
                CurrentLyric = await _context.Lyrics.FirstOrDefaultAsync(m => m.LyricId == id),
                RelatedLyric = randomLyrics,
            };


            

            //GET THE MEDIATYPE COUNT BY CURRENT MEDIATYPE USE MEDIATYPEID
            //          var mediatypeswithcountsbymediatype = await _context.Musics.Where(s => s.MediaTypeId == mediatypeid)
            //.GroupBy(s => s.MediaType)
            //.Select(group => new { MediaType = group.Key, Count = group.Count() })
            //.ToListAsync();
            var musictypeswithcountsbymediatype = await _context.Musics.OrderByDescending(m => randomLyricIds.Contains(m.MusicId)).Include(m => m.MediaType).Include(m => m.Lyric).Where(m => m.Lyric.LyricTypeId == lyrictypeid).Take(4).ToListAsync();

            ViewBag.musicbymediatype = musictypeswithcountsbymediatype;

            return View(viewModel);
        }

        // GET: Lyrics/Create
        public IActionResult Create()
        {
            ViewData["LyricTypeId"] = new SelectList(_context.LyricTypes, "LyricTypeId", "Type");
            return View();
        }

        // POST: Lyrics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LyricId,Title,LyricMediaLink,LyricThumbnail,LyricTypeId")] Lyric lyric)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lyric);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LyricTypeId"] = new SelectList(_context.LyricTypes, "LyricTypeId", "Type", lyric.LyricTypeId);
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
            ViewData["LyricTypeId"] = new SelectList(_context.LyricTypes, "LyricTypeId", "Type", lyric.LyricTypeId);
            return View(lyric);
        }

        // POST: Lyrics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LyricId,Title,LyricMediaLink,LyricThumbnail,LyricTypeId")] Lyric lyric)
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
            ViewData["LyricTypeId"] = new SelectList(_context.LyricTypes, "Type", "LyricTypeId", lyric.LyricTypeId);
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
                .Include(l => l.LyricType)
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

        //write a method to fetch all blobs from the azure storage account
        public async Task<List<BlobInfo>> FetchBlobs()
        {
            var connectionString = _configuration.GetConnectionString("storageconnection");
            string accountName = "dctstorageplace";
            string containerName = "dctlyrics";
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            List<BlobInfo> blobInfoList = new List<BlobInfo>();

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                blobInfoList.Add(new BlobInfo
                {
                    FileName = blobItem.Name,
                    Url = $"https://{accountName}.blob.core.windows.net/{containerName}/{blobItem.Name}",
                    LastModified = blobItem.Properties.LastModified?.DateTime,
                    SizeInBytes = blobItem.Properties.ContentLength
                });
            }

            return blobInfoList;
        }



        public class BlobInfo
        {
            public string FileName { get; set; }
            public string Url { get; set; }
            public DateTime? LastModified { get; set; }
            public long? SizeInBytes { get; set; }
        }

        public async Task<IActionResult> DownloadLyric(int? id)
        {
            if (id == null || _context.Lyrics == null)
            {
                return NotFound();
            }

            var lyric = await _context.Lyrics
                .Include(s => s.LyricType)
                .FirstOrDefaultAsync(s => s.LyricId == id);

            if (lyric == null)
            {
                return NotFound();
            }

            // Fetch all blobs
            var availableBlobs = await FetchBlobs();
            if (availableBlobs == null || !availableBlobs.Any())
            {
                return NotFound();
            }

            // Find the blob that matches the requested lyric
            // Find the blob that matches the requested lyric by comparing the full file path or file name
            BlobInfo lyricBlobInfo = availableBlobs
                .FirstOrDefault(blob =>
                {
                    // Normalize the blob's filename by replacing spaces and trimming paths if needed
                    var normalizedBlobFileName = Path.GetFileName(blob.FileName)
                                                    .Replace(" ", "")
                                                    .ToLowerInvariant();

                    // Normalize the lyric title for comparison
                    var normalizedlyricTitle = lyric.Title
                                                  .Replace(" ", "")
                                                  .ToLowerInvariant();

                    // Check if the blob file name contains the normalized lyric title
                    return normalizedBlobFileName.Contains(normalizedlyricTitle);
                });


            if (lyricBlobInfo == null)
            {
                return NotFound();
            }

            try
            {
                // Convert URL string to Uri object
                var blobClient = new BlobClient(new Uri(lyricBlobInfo.Url));

                // Download the blob as a stream
                var downloadInfo = await blobClient.DownloadAsync();
                var stream = downloadInfo.Value.Content;

                // Return the stream to the client for download without loading the file into memory
                return File(stream, "application/octet-stream", lyricBlobInfo.FileName);
            }
            catch (Exception ex)
            {
                // Log the error for troubleshooting
                Console.WriteLine($"Error downloading file: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while downloading the file." });
            }
        }


        // Helper method to extract the relevant part from the filename for comparison
        string ExtractRelevantBlobName(string fileName)
        {
            // Split the filename by spaces or dashes to isolate potential title parts
            var parts = fileName.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);

            // Reconstruct the name by excluding known non-title elements (like dates)
            var relevantParts = parts.Where(part => !IsDateOrCommonWord(part)).ToList();

            // Join the relevant parts back into a single string for comparison
            return string.Join(" ", relevantParts).Trim();
        }

        // Helper method to determine if a part is a date or a common word that should be excluded
        bool IsDateOrCommonWord(string part)
        {
            // Try to parse the part as a date
            DateTime dateValue;
            if (DateTime.TryParse(part, out dateValue))
            {
                return true;
            }

            // Add any other common words that shouldn't be part of the title (e.g., "By", "Sermon", etc.)
            var commonWords = new[] { "by", "sermon", "audio", "video" };

            return commonWords.Contains(part.ToLower());
        }
    }
}
