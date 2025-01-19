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
using System.IO.Compression;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace DCTStore.Controllers
{
    public class SermonsController : Controller
    {
        private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;

		public SermonsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
			_configuration = configuration;
		}

		// GET : ThankYou
		public IActionResult ThankYou(int? id)
		{
			ViewData["DownloadId"] = id; // Pass the ID to the view
			return View();
		}

		public IActionResult ThankYouS(int? id)
		{
			ViewData["DownloadId"] = id; // Pass the ID to the view
			return View();
		}

		public async Task<IActionResult> GetSermonByMinister(string mediaType, int page = 1, int pageSize = 9)
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
				.Where(s => s.MediaType.Type == mediaType)
				.GroupBy(s => s.SermonType)
				.Select(group => new { SermonType = group.Key, Count = group.Count() })
				.ToListAsync();


			//pass the sermon type count by minister name to the view
			ViewBag.SermonTypesByMinister = sermonTypesWithCountsByMinister;

			var totalItems = await _context.Sermons
	.Where(m => m.Minister.Name == mediaType)
	.CountAsync();

			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var pagedSermons = await _context.Sermons
				.Include(s => s.MediaType)
				.Include(s => s.Minister)
				.Include(s => s.SermonType)
				.Where(m => m.Minister.Name == mediaType)
				.OrderByDescending(m => m.DatePreached)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			ViewBag.SermonTypes = sermonTypesWithCounts;
			ViewBag.RecentSermons = latestUploads;
			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;
			ViewData["Title"] = mediaType; // Set the title for the view

			return View(pagedSermons);
		}

        public async Task<IActionResult> GetSermonByMediaType(string mediaType, int page = 1, int pageSize = 9)
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

			var totalItems = await _context.Sermons
                .Where(m => m.MediaType.Type == mediaType)
                .CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedSermons = await _context.Sermons
                .Include(s => s.MediaType)
                .Include(s => s.Minister)
                .Include(s => s.SermonType)
                .Where(m => m.MediaType.Type == mediaType)
                .OrderByDescending(m => m.DatePreached)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.SermonTypes = sermonTypesWithCounts;
            ViewBag.RecentSermons = latestUploads;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewData["Title"] = mediaType; // Set the title for the view

            return View(pagedSermons);
        }


		// GET: Sermons
		public async Task<IActionResult> Index(int page = 1, int pageSize = 9)
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
				.Take(6)
				.ToListAsync();

			var totalItems = await _context.Sermons.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var pagedSermons = await _context.Sermons
				.Include(s => s.MediaType)
				.Include(s => s.Minister)
				.Include(s => s.SermonType)
				.OrderByDescending(s => s.DatePreached)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			ViewBag.SermonTypes = sermonTypesWithCounts;
			ViewBag.RecentSermons = latestUploads;
			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;

			return View(pagedSermons);
		}


		// Get: Local Sermons
		public async Task<IActionResult> LocalIndex(int page = 1, int pageSize = 9)
		{
			ViewBag.SermonTypes = await _context.Sermons
				.GroupBy(m => m.MediaType)
				.Select(group => new { MediaType = group.Key, Count = group.Count() })
				.ToListAsync();

			ViewBag.RecentSermons = await _context.Sermons
				.Include(s => s.MediaType)
				.Include(s => s.Minister)
				.OrderByDescending(s => s.DatePreached)
				.Take(8)
				.ToListAsync();

            //add a viewbag that will hold the ministers
            ViewBag.Ministers = await _context.Ministers
				.Where(m => m.MininsterTypeId == 1)
				.OrderBy(x => Guid.NewGuid())
				.Take(6)
				.ToListAsync();

			var totalItems = await _context.Sermons
				.Where(s => s.Minister.MininsterTypeId == 1)
				.CountAsync();

			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var pagedSermons = await _context.Sermons
				.Include(s => s.Minister)
				.Include(s => s.MediaType)
				.Where(s => s.Minister.MininsterTypeId == 1)
				.OrderByDescending(s => s.DatePreached)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;

			return View(pagedSermons);
		}


		// Get: Visitor Sermons
		public async Task<IActionResult> VisitorsIndex(int page = 1, int pageSize = 9)
		{
			ViewBag.SermonTypes = await _context.Sermons
				.GroupBy(m => m.MediaType)
				.Select(group => new { MediaType = group.Key, Count = group.Count() })
				.ToListAsync();

			ViewBag.RecentSermons = await _context.Sermons
				.Include(s => s.MediaType)
				.Include(s => s.Minister)
				.OrderByDescending(s => s.DatePreached)
				.Take(8)
				.ToListAsync();

			//add a viewbag that will hold the ministers
			ViewBag.Ministers = await _context.Ministers
				.Where(m => m.MininsterTypeId == 2 && m.MinisterId !=7)
				.OrderBy(x => Guid.NewGuid())
				.Take(10)
				.ToListAsync();

			
			var totalItems = await _context.Sermons
				.Where(s => s.Minister.MininsterTypeId == 2)
				.CountAsync();

			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			var pagedSermons = await _context.Sermons
				.Include(s => s.Minister)
				.Include(s => s.MediaType)
				.Where(s => s.Minister.MininsterTypeId == 2)
				.OrderByDescending(s => s.DatePreached)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;

			return View(pagedSermons);
		}

		// Get: Local Sermons Topics
		public async Task<IActionResult> LocalIndexTopics(int page = 1, int pageSize = 9)
        {
			ViewBag.SermonTypes = await _context.Sermons
				.GroupBy(m => m.MediaType)
				.Select(group => new { MediaType = group.Key, Count = group.Count() })
				.ToListAsync();

			ViewBag.RecentSermons = await _context.Sermons
				.Include(s => s.MediaType)
				.Include(s => s.Minister)
				.OrderByDescending(s => s.DatePreached)
				.Take(8)
				.ToListAsync();

			//add a viewbag that will hold the ministers
			ViewBag.Ministers = await _context.Ministers
				.Where(m => m.MininsterTypeId == 1)
				.OrderBy(x => Guid.NewGuid())
				.Take(10)
				.ToListAsync();

			var totalItems = await _context.Sermons
			   .Where(s => s.Minister.MininsterTypeId == 1)
			   .CountAsync();

			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			// Get the latest 8 sermons, from each select the type and image of the sermon except for Krefeld Preaching type, take 1 sermon per type
			var pagedSermons = _context.Sermons
				.Where(s => s.Minister.MininsterTypeId == 1 && s.MediaType.Type != "Krefeld Preaching")
			.GroupBy(s => s.SermonTypeId)
			.Select(g => new SermonSubjects
			{
				SermonTypeId = g.Key.Value,
				SermonType = g.First().SermonType.Type,
				MediaType = g.First().MediaType,
				DatePreached = g.First().DatePreached,
				MinisterName = g.First().Minister.Name
			})
			.OrderByDescending(s => s.DatePreached)
			.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;

			return View(pagedSermons);
		}

		// Get: Sermons by Selected Sermon Type
		public async Task<IActionResult> SermonsByType(int? id, int page = 1, int pageSize = 9)
		{

			if (id == null || _context.Sermons == null)
			{
				return NotFound();
			}

			var sermonTypesWithCounts = await _context.Sermons
	.GroupBy(m => m.MediaType)
	.Select(group => new { MediaType = group.Key, Count = group.Count() })
	.ToListAsync();
			var latestUploads = await _context.Sermons.OrderByDescending(m => m.DatePreached).Take(3).ToListAsync();
			var totalItems = await _context.Sermons.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			ViewBag.SermonTypes = sermonTypesWithCounts;

			//create a viewbag that will hold the sermons by seromn type
			ViewBag.SermonsBySermonType = _context.Sermons
				.Include(s => s.MediaType)
				.Include(s => s.Minister)
				.Include(s => s.SermonType)
				.GroupBy(s => s.SermonType)
				.ToList();
			//  and a second viewbag that will hold recent uploaded sermons
			ViewBag.RecentSermons = _context.Sermons
				.Include(s => s.MediaType)
				.Include(s => s.Minister)
				.OrderByDescending(s => s.DatePreached)
				.Take(8)
				.ToList();

			

			//add a viewbag that will hold the ministers
			ViewBag.Ministers = await _context.Ministers
				.Where(m => m.MininsterTypeId == 1)
				.OrderBy(x => Guid.NewGuid())
				.Take(10)
				.ToListAsync();

			//get the sermon type by the id
			var sermonType = await _context.Sermons.Where(m => m.SermonTypeId == id).Select(m => m.SermonType.Type).FirstOrDefaultAsync();

			ViewData["Title"] = sermonType + " Sermon Topics"; // Set the title for the view

			var applicationDbContext = _context.Sermons.Include(s => s.MediaType).Include(s => s.Minister).Include(s => s.SermonType).Where(s => s.SermonTypeId == id);
			return View(await applicationDbContext.ToListAsync());

		}

        public async Task<IActionResult> GetSermonBySermonType(string mediaType, int page = 1, int pageSize = 9)
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

			var totalItems = await _context.Sermons
                .Where(m => m.MediaType.Type == mediaType)
                .CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedSermons = await _context.Sermons
                .Include(s => s.MediaType)
                .Include(s => s.Minister)
                .Include(s => s.SermonType)
                .Where(m => m.SermonType.Type == mediaType)
                .OrderByDescending(m => m.DatePreached)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.SermonTypes = sermonTypesWithCounts;
            ViewBag.RecentSermons = latestUploads;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewData["Title"] = mediaType; // Set the title for the view

            return View(pagedSermons);
        }



        // Get: Visitor Sermons Topics
        public async Task<IActionResult> VisitorsIndexTopics(int page = 1, int pageSize = 9)
        {
			ViewBag.SermonTypes = await _context.Sermons
				.GroupBy(m => m.MediaType)
				.Select(group => new { MediaType = group.Key, Count = group.Count() })
				.ToListAsync();

			ViewBag.RecentSermons = await _context.Sermons
				.Include(s => s.MediaType)
				.Include(s => s.Minister)
				.OrderByDescending(s => s.DatePreached)
				.Take(8)
				.ToListAsync();

			//add a viewbag that will hold the ministers
			ViewBag.Ministers = await _context.Ministers
				.Where(m => m.MininsterTypeId == 2 && m.MinisterId != 7)
				.OrderBy(x => Guid.NewGuid())
				.Take(10)
				.ToListAsync();

			
		

            var totalItems = await _context.Sermons
				.Where(s => s.Minister.MininsterTypeId == 2)
				.CountAsync();

			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			// Get the latest 8 sermons, from each select the type and image of the sermon except for Krefeld Preaching type, take 1 sermon per type
			var pagedSermons = _context.Sermons
				.Where(s => s.Minister.MininsterTypeId == 2 && s.MediaType.Type != "Krefeld Preaching")
			.GroupBy(s => s.SermonTypeId)
			.Select(g => new SermonSubjects
			{
				SermonTypeId = g.Key.Value,
				SermonType = g.First().SermonType.Type,
				MediaType = g.First().MediaType,
				DatePreached = g.First().DatePreached,
				MinisterName = g.First().Minister.Name
			})
			.OrderByDescending(s => s.DatePreached)
			.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToList();

			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;

			return View(pagedSermons);
        }

		//write a method to fetch all blobs from the azure storage account
		public async Task<List<BlobInfo>> FetchBlobs()
		{
			var connectionString = _configuration.GetConnectionString("storageconnection");
			string accountName = "dctstorageplace";
			string containerName = "audio-recordings";
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

        // create download action to download all the sermons by the selected sermon type in a zip file
        public async Task<IActionResult> DownloadSermonsByType(int? id)
        {
            if (id == null || _context.Sermons == null)
            {
                return NotFound();
            }

            var sermons = await _context.Sermons
                .Include(s => s.MediaType)
                .Include(s => s.Minister)
                .Include(s => s.SermonType)
                .Where(s => s.SermonTypeId == id)
                .ToListAsync();

            if (!sermons.Any())
            {
                return NotFound();
            }

            // Find the sermon type to get the pre-zipped download link
            var sermonType = await _context.SermonType.FirstOrDefaultAsync(s => s.SermonTypeId == id);

            if (sermonType == null || string.IsNullOrEmpty(sermonType.TypeDownloadLink))
            {
                return NotFound();
            }

            try
            {
                // Convert the pre-zipped file URL into a BlobClient
                var blobClient = new BlobClient(new Uri(sermonType.TypeDownloadLink));

                // Download the pre-zipped file as a stream
                var downloadInfo = await blobClient.DownloadAsync();
                var stream = downloadInfo.Value.Content;

                // Set the file name (you can modify this as needed)
                var fileName = $"Sermons_{sermonType.Type}.zip";

                // Return the stream for download
                return File(stream, "application/zip", fileName);
            }
            catch (Exception ex)
            {
                // Log the error for troubleshooting
                Console.WriteLine($"Error downloading pre-zipped file: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while downloading the pre-zipped file." });
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

        // create download action to download a sermon based on the sermon id
        public async Task<IActionResult> DownloadSermon(int? id)
        {
            if (id == null || _context.Sermons == null)
            {
                return NotFound();
            }

            var sermon = await _context.Sermons
                .Include(s => s.MediaType)
                .Include(s => s.Minister)
                .Include(s => s.SermonType)
                .FirstOrDefaultAsync(s => s.SermonId == id);

            if (sermon == null)
            {
                return NotFound();
            }

            // Fetch all blobs
            var availableBlobs = await FetchBlobs();
            if (availableBlobs == null || !availableBlobs.Any())
            {
                return NotFound();
            }

            // Find the blob that matches the requested sermon
            BlobInfo sermonBlobInfo = availableBlobs
                .FirstOrDefault(blob =>
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(blob.FileName);
                    var relevantBlobNamePart = ExtractRelevantBlobName(fileNameWithoutExtension);

                    return relevantBlobNamePart.Equals(sermon.Title, StringComparison.OrdinalIgnoreCase) ||
                           blob.FileName.Contains(sermon.DatePreached.ToString("dd.MM.yyyy"));
                });

            if (sermonBlobInfo == null)
            {
                return NotFound();
            }

            try
            {
                // Convert URL string to Uri object
                var blobClient = new BlobClient(new Uri(sermonBlobInfo.Url));

                // Download the blob as a stream
                var downloadInfo = await blobClient.DownloadAsync();
                var stream = downloadInfo.Value.Content;

                // Return the stream to the client for download without loading the file into memory
                return File(stream, "application/octet-stream", sermonBlobInfo.FileName);
            }
            catch (Exception ex)
            {
                // Log the error for troubleshooting
                Console.WriteLine($"Error downloading file: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while downloading the file." });
            }
        }








        // Get: Krefeld Sermons
        public async Task<IActionResult> KrefeldIndex(int page = 1, int pageSize = 9)
		{
			//create a viewbag that will hold the sermons by seromn type
			ViewBag.SermonsBySermonType = _context.Sermons
				.Include(s => s.MediaType)
				.Include(s => s.Minister)
				.Include(s => s.SermonType)
				.GroupBy(s => s.SermonType)
				.ToList();
			//  and a second viewbag that will hold recent uploaded sermons
			ViewBag.RecentSermons = _context.Sermons
				.Include(s => s.MediaType)
				.Include(s => s.Minister)
				.OrderByDescending(s => s.DatePreached)
				.Take(8)
				.ToList();

			// Get all krefeld sermons only
			var sermons = _context.Sermons
				.Include(s => s.MediaType)
				.Include(s => s.Minister)
				.Where(s => s.MediaType.Type == "Krefeld Preaching")
				.ToList();

			var totalItems = sermons.Count();
			var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

			ViewBag.TotalPages = totalPages;
			ViewBag.CurrentPage = page;

			return View(sermons);
		}

		// GET: Sermons/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sermons == null)
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

            //get minister id of the current sermon
            var ministerId = _context.Sermons
                .Where(s => s.SermonId == id)
                .Select(s => s.MinisterId)
                .FirstOrDefault();

            //get the sermontype count by minister name of the current sermon use ministerid
            var sermonTypesWithCountsByMinister = await _context.Sermons
                .Where(s => s.MinisterId == ministerId)
                .GroupBy(s => s.SermonType)
                .Select(group => new { SermonType = group.Key, Count = group.Count() })
                .ToListAsync();
            

            //pass the sermon type count by minister name to the view
            ViewBag.SermonTypesByMinister = sermonTypesWithCountsByMinister;


            //get the count of sermon type in the database
            var sermonTypeCount = await _context.Sermons
                .GroupBy(s => s.SermonType)
                .Select(group => new { SermonType = group.Key, Count = group.Count() })
                .ToListAsync();

            //get the sermontype id based on the sermon id
            var sermonTypeId = _context.Sermons
                .Where(s => s.SermonId == id)
                .Select(s => s.SermonTypeId)
                .FirstOrDefault();

            ViewBag.SermonTypes = sermonTypesWithCounts;

			//create a viewbag that will hold the sermons by seromn type
			ViewBag.SermonsBySermonType = _context.Sermons
				.Include(s => s.MediaType)
				.Include(s => s.Minister)
				.Include(s => s.SermonType)
				.GroupBy(s => s.SermonType)
				.ToList();
			//  and a second viewbag that will hold recent uploaded sermons
			ViewBag.RecentSermons = _context.Sermons
				.Include(s => s.MediaType)
				.Include(s => s.Minister)
				.OrderByDescending(s => s.DatePreached)
				.Take(5)
				.ToList();

            // a viewbag that will hold the related sermon type
            ViewBag.RelatedSermons = _context.Sermons
                .Include(s => s.MediaType)
                .Include(s => s.Minister)
                .Include(s => s.SermonType)
                .Where(s => s.SermonTypeId == sermonTypeId)
                .OrderBy(s => s.DatePreached)
                .Take(5)
                .ToList();



            // Select 4 random sermons from the database by sermon type of the current id
            //var randomSermons = _context.Sermons
            //	.Where(s => s.SermonTypeId == id)
            //	.OrderBy(s => s.DatePreached)
            //	.Take(4)
            //	.ToList();

            //genertae 5 random numbers from 1 to the sermon type count
            var random = new Random();
            var randomSermonIds = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                randomSermonIds.Add(random.Next(1, sermonTypeCount.Count));
            }

           

            var randomSermons= _context.Sermons.Include(s => s.MediaType).Include(s => s.Minister).Include(s => s.SermonType).Where(s => s.SermonTypeId.HasValue && randomSermonIds.Contains(s.SermonTypeId.Value)).Take(4).ToList();

           

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

            //get the minister type id of the current sermon
            var ministerTypeId = _context.Ministers
				.Where(m => m.MinisterId == sermon.MinisterId)
				.Select(m => m.MininsterTypeId)
				.FirstOrDefault();

            //get the ministers by the minister type id
            var ministers = _context.Ministers
				.Where(m => m.MininsterTypeId == ministerTypeId && m.MinisterId != 7)
				.OrderBy(x => Guid.NewGuid())
				.Take(6)
				.ToList();

            ViewBag.Ministers = ministers;

           

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
