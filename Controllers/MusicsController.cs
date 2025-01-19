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
using DocumentFormat.OpenXml.Office2010.Excel;
using Azure.Storage.Blobs;
using DCTStore.Data.Migrations;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace DCTStore.Controllers
{
	public class MusicsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;

		public MusicsController(ApplicationDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		public IActionResult ThankYouS(int? id)
		{
			ViewData["DownloadId"] = id; // Pass the ID to the view
			return View();
		}

		// GET: Musics
		public async Task<IActionResult> Index(int page = 1, int pageSize = 9)
		{
			var lyricTypesWithCounts = await _context.Lyrics
				.GroupBy(m => m.LyricType)
				.Select(group => new { LyricType = group.Key, Count = group.Count() })
				.ToListAsync();

			

			var musicTypesWithCounts = await _context.Musics
	.GroupBy(m => m.MediaType)
	.Select(group => new { MediaType = group.Key, Count = group.Count() })
	.ToListAsync();
			var latestUploads = await _context.Musics.OrderByDescending(m => m.DateUploaded).Take(3).ToListAsync();

			var totalItems = await _context.Musics.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			ViewBag.MusicTypes = musicTypesWithCounts;
			ViewBag.LyricTypes = lyricTypesWithCounts;
			ViewBag.LatestUploads = latestUploads;
			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;
			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;
			ViewData["Title"] = "DCT Music"; // Set the title for the view

			var applicationDbContext = _context.Musics.Include(m => m.Lyric).Include(m => m.MediaType).Skip((page - 1) * pageSize)
				.Take(pageSize);
			return View(await applicationDbContext.ToListAsync());
		}

		// GET: Musics/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			var musicTypesWithCounts = await _context.Musics
	.GroupBy(m => m.MediaType)
	.Select(group => new { MediaType = group.Key, Count = group.Count() })
	.ToListAsync();
			ViewBag.MusicTypes = musicTypesWithCounts;


			var latestUploads = await _context.Musics.OrderByDescending(m => m.DateUploaded).Take(4).ToListAsync();

			
			ViewBag.LatestUploads = latestUploads;

			if (id == null || _context.Musics == null)
			{
				return NotFound();
			}

			//GET MEDIATYPE ID OF CURRENT MUSIC
			var mediatypeid= _context.Musics.Where(s => s.MusicId == id).Select(s => s.MediaTypeId).FirstOrDefault();




			//GET THE MEDIATYPE COUNT BY CURRENT MEDIATYPE USE MEDIATYPEID
			//          var mediatypeswithcountsbymediatype = await _context.Musics.Where(s => s.MediaTypeId == mediatypeid)
			//.GroupBy(s => s.MediaType)
			//.Select(group => new { MediaType = group.Key, Count = group.Count() })
			//.ToListAsync();
			var mediatypeswithcountsbymediatype = await _context.Musics.OrderByDescending(m => m.DateUploaded).Where(m => m.MediaTypeId == mediatypeid).Take(4).ToListAsync();

			ViewBag.musicbymediatype = mediatypeswithcountsbymediatype;

			// Generate 5 random Music IDs from the database
			var randomMusicIds = _context.Musics
				.Select(m => m.MusicId) // Select only the Music IDs
				.OrderBy(x => Guid.NewGuid()) // Randomize the order
				.Take(5) // Take 5 random IDs
				.ToList();

			// Select 4 random music records by matching with the randomMusicIds
			var randomMusic = _context.Musics
				.Where(m => randomMusicIds.Contains(m.MusicId)) // Filter by the random Music IDs
				.Include(m => m.MediaType)
				.Include(m => m.Lyric)
				.Take(4) // Limit to 4 results
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

			var lyrictypeid = _context.Musics.Where(s => s.MusicId == id).Select(s => s.Lyric.LyricTypeId).FirstOrDefault();

			// Generate 4 random lyrics by lyrictypeid
			var randomLyrics = await _context.Lyrics
				.Where(l => l.LyricTypeId == lyrictypeid)
				.OrderBy(x => Guid.NewGuid())
				.Take(4)
				.ToListAsync();

			ViewBag.lyricsbymediatype = randomLyrics;

			

			// Return the view with the view model
			return View(viewModel);
		}

		

		//write a method to fetch all blobs from the azure storage account
		public async Task<List<BlobInfo>> FetchBlobs()
		{
			var connectionString = _configuration.GetConnectionString("storageconnection");
			string accountName = "dctstorageplace";
			string containerName = "dctmusic";
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

		public async Task<IActionResult> DownloadMusic(int? id)
		{
			if (id == null || _context.Musics == null)
			{
				return NotFound();
			}

			var music = await _context.Musics
				.Include(s => s.MediaType)
				.Include(s => s.Lyric)
				.FirstOrDefaultAsync(s => s.MusicId == id);

			if (music == null)
			{
				return NotFound();
			}

			// Fetch all blobs
			var availableBlobs = await FetchBlobs();
			if (availableBlobs == null || !availableBlobs.Any())
			{
				return NotFound();
			}

			// Normalize the music title for comparison, removing spaces and converting to lowercase
			var normalizedMusicTitle = music.Title.Replace(" ", "").ToLowerInvariant();

			// Find the blob that exactly matches the requested music title
			BlobInfo musicBlobInfo = availableBlobs
				.FirstOrDefault(blob =>
				{
					// Normalize the blob's filename by trimming paths, removing spaces, and converting to lowercase
					var normalizedBlobFileName = Path.GetFileNameWithoutExtension(blob.FileName)
													  .Replace(" ", "")
													  .ToLowerInvariant();

					// Ensure that the blob filename exactly matches the music title
					return string.Equals(normalizedBlobFileName, normalizedMusicTitle, StringComparison.OrdinalIgnoreCase);
				});

			if (musicBlobInfo == null)
			{
				return NotFound(); // Handle case where no exact matching blob is found
			}

			try
			{
				// Convert URL string to Uri object
				var blobClient = new BlobClient(new Uri(musicBlobInfo.Url));

				// Download the blob as a stream
				var downloadInfo = await blobClient.DownloadAsync();
				var stream = downloadInfo.Value.Content;

				// Return the stream to the client for download without loading the file into memory
				return File(stream, "application/octet-stream", Path.GetFileName(musicBlobInfo.FileName));
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

		public async Task<IActionResult> GetSongsByMediaType(string mediaType, int page = 1, int pageSize = 9)
		{
			var lyricTypesWithCounts = await _context.Lyrics
				.GroupBy(m => m.LyricType)
				.Select(group => new { LyricType = group.Key, Count = group.Count() })
				.ToListAsync();

			var musicTypesWithCounts = await _context.Musics
				.GroupBy(m => m.MediaType)
				.Select(group => new { MediaType = group.Key, Count = group.Count() })
				.ToListAsync();

			var latestUploads = await _context.Musics
				.OrderByDescending(m => m.DateUploaded)
				.Take(3)
				.ToListAsync();

			// Total number of records for the specified media type
			var totalItems = await _context.Musics
				.Where(m => m.MediaType.Type == mediaType)
				.CountAsync();

			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			// Pagination: Skip records based on page number and take the next set of pageSize
			var applicationDbContext = await _context.Musics
				.Include(m => m.Lyric)
				.Include(m => m.MediaType)
				.Where(m => m.MediaType.Type == mediaType)
				.OrderBy(m => m.MediaTypeId) // Assuming you have an Id field to order by
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			ViewBag.MusicTypes = musicTypesWithCounts;
			ViewBag.LyricTypes = lyricTypesWithCounts;
			ViewBag.LatestUploads = latestUploads;
			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;
			ViewData["Title"] = mediaType; // Set the title for the view

			return View(applicationDbContext);
		}



		// GET: Musics/Create
		public IActionResult Create()
		{
			ViewData["LyricId"] = new SelectList(_context.Lyrics, "LyricId", "Title");
			ViewData["MediaTypeId"] = new SelectList(_context.MediaTypes, "MediaTypeId", "Type");
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
			ViewData["LyricId"] = new SelectList(_context.Lyrics, "LyricId", "Title", music.LyricId);
			ViewData["MediaTypeId"] = new SelectList(_context.MediaTypes, "MediaTypeId", "Type", music.MediaTypeId);
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
			ViewData["LyricId"] = new SelectList(_context.Lyrics, "LyricId", "Title", music.LyricId);
			ViewData["MediaTypeId"] = new SelectList(_context.MediaTypes, "MediaTypeId", "Type", music.MediaTypeId);
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
			ViewData["LyricId"] = new SelectList(_context.Lyrics, "LyricId", "Title", music.LyricId);
			ViewData["MediaTypeId"] = new SelectList(_context.MediaTypes, "MediaTypeId", "Type", music.MediaTypeId);
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
