using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using DCTStore.Data;
using DCTStore.Models;
using DCTStore.Repository;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Cart _cart;
    private readonly IConfiguration _configuration;

    public CartController(ApplicationDbContext context, IHttpClientFactory httpClientFactory, Cart cart, IConfiguration configuration)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _cart = cart;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        var cartItems = _cart.GetItems();
        var cartViewModel = new CartViewModel
        {
            CartItems = cartItems
        };
        return View(cartViewModel);
    }

	public IActionResult AddToCart(int itemId, string itemCategory, string title, string mediaLink, string? videoMediaLink = null, string imagemedia = "")
	{
		try
		{
			_cart.AddItem(itemId, itemId, itemCategory, title, mediaLink, videoMediaLink, imagemedia);
			return Json(new { success = true });
		}
		catch (Exception ex)
		{
			// Log the exception for debugging purposes
			Console.WriteLine(ex.Message);
			return Json(new { success = false, message = "An error occurred while adding to cart." });
		}
	}

    public IActionResult ClearCart()
    {
        _cart.Clear();
        return RedirectToAction("Index");
    }

    public IActionResult RemoveFromCart(int cartItemId)
    {
        _cart.RemoveItem(cartItemId);

        return RedirectToAction("Index");
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

    public int icheck = 0;
    // Global list to store files and their names
    private List<(Stream stream, string fileName, string contentType)> filesToCompress = new List<(Stream, string, string contentType)>();

    public async Task<IActionResult> DownloadItems()
    {
        // Get the items in the cart (you need to implement your own cart system)
        var cartItems = _cart.GetItems();
        
        foreach (var cartItem in cartItems)
        {
            if (cartItem.ItemCategory == "Lyric")
            {
                await DownloadLyric(cartItem.ItemId,cartItem.Title,cartItem.MediaLink);
            }
            else if (cartItem.ItemCategory == "Sermon")
            {
                await DownloadSermon(cartItem.ItemId, cartItem.Title, cartItem.MediaLink);
            }
            else if (cartItem.ItemCategory == "Sermon Collection")
            {
                await DownloadSermonsByType(cartItem.ItemId, cartItem.Title, cartItem.MediaLink);
            }
            else if (cartItem.ItemCategory == "Music")
            {
                await DownloadMusic(cartItem.ItemId, cartItem.Title, cartItem.MediaLink);
            }
        }

        // After collecting all files, compress them and return as ZIP
        var zipFile = await CreateZipFileAsync();

        // Return the ZIP file to the user
        return File(zipFile, "application/zip", "DownloadedItems.zip");

      

      
    }

    private async Task<MemoryStream> CreateZipFileAsync()
    {
        var zipStream = new MemoryStream();

        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            foreach (var (stream, fileName, contentType) in filesToCompress)
            {
                var zipEntry = archive.CreateEntry(fileName);

                using (var entryStream = zipEntry.Open())
                {
                    await stream.CopyToAsync(entryStream);
                }

                stream.Close(); // Close individual file stream after copying to ZIP
            }
        }

        zipStream.Position = 0; // Reset the memory stream position for download
        return zipStream;
    }


    public async Task DownloadMusic(int? id,string? title, string? link)
    {
        if (id == null || _context.Musics == null)
        {
            Console.WriteLine(" context is null.");
        }

        var music = await _context.Musics
            .Include(s => s.MediaType)
            .Include(s => s.Lyric)
            .FirstOrDefaultAsync(s => s.MusicId == id);

        if (music == null)
        {
            Console.WriteLine(" context is null.");
        }

        // Fetch all blobs
        var availableBlobs = await FetchBlobs();
        if (availableBlobs == null || !availableBlobs.Any())
        {
            Console.WriteLine(" context is null.");
        }

        // Normalize the music title for comparison, removing spaces and converting to lowercase
        var normalizedMusicTitle = title.Replace(" ", "").ToLowerInvariant();

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
            Console.WriteLine(" context is null."); // Handle case where no exact matching blob is found
        }

        try
        {
            // Convert URL string to Uri object
            var blobClient = new BlobClient(new Uri(link));

            // Download the blob as a stream
            var downloadInfo = await blobClient.DownloadAsync();
            var stream = downloadInfo.Value.Content;
            

            // Return the stream to the client for download without loading the file into memory
            //return File(stream, "application/octet-stream", Path.GetFileName(musicBlobInfo.FileName));
            //filesToCompress.Add((stream, "application/octet-stream", Path.GetFileName(title)));
            filesToCompress.Add((stream, $"{title}.mp3", "audio/mpeg"));
        }
        catch (Exception ex)
        {
            // Log the error for troubleshooting
            Console.WriteLine($"Error downloading file: {ex.Message}");
            
        }
    }

    public async Task DownloadLyric(int? id, string? title, string? link)
    {
        if (id == null || _context.Lyrics == null)
        {
            Console.WriteLine(" context is null.");
        }

        var lyric = await _context.Lyrics
            .Include(s => s.LyricType)
            .FirstOrDefaultAsync(s => s.LyricId == id);

        if (lyric == null)
        {
            Console.WriteLine(" context is null.");
        }

        // Fetch all blobs
        var availableBlobs = await FetchBlobs();
        if (availableBlobs == null || !availableBlobs.Any())
        {
            Console.WriteLine(" context is null.");
        }

        // Find the blob that matches the requested lyric
        // Find the blob that matches the requested lyric by comparing the full file path or file name
        BlobInfo lyricBlobInfo = availableBlobs
            .FirstOrDefault(blob =>
            {
                // Normalize the blob's filename by replacing spaces and trimming paths if needed
                var normalizedBlobFileName = Path.GetFileName(title)
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
            Console.WriteLine(" context is null.");
        }

        try
        {
            // Convert URL string to Uri object
            var blobClient = new BlobClient(new Uri(link));

            // Download the blob as a stream
            var downloadInfo = await blobClient.DownloadAsync();
            var stream = downloadInfo.Value.Content;

            // Return the stream to the client for download without loading the file into memory
            //return File(stream, "application/octet-stream", lyricBlobInfo.FileName);
            //filesToCompress.Add((stream, "application/octet-stream", title));
            filesToCompress.Add((stream, $"{title}.pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"));
        }
        catch (Exception ex)
        {
            // Log the error for troubleshooting
            Console.WriteLine($"Error downloading file: {ex.Message}");
           
        }
    }

    public async Task DownloadSermonsByType(int? id, string? title, string? link)
    {
        if (id == null || _context.Sermons == null)
        {
            Console.WriteLine(" context is null.");
        }

        var sermons = await _context.Sermons
            .Include(s => s.MediaType)
            .Include(s => s.Minister)
            .Include(s => s.SermonType)
            .Where(s => s.SermonTypeId == id)
            .ToListAsync();

        if (!sermons.Any())
        {
            Console.WriteLine(" context is null.");
        }

        // Find the sermon type to get the pre-zipped download link
        var sermonType = await _context.SermonType.FirstOrDefaultAsync(s => s.SermonTypeId == id);

        if (sermonType == null || string.IsNullOrEmpty(sermonType.TypeDownloadLink))
        {
            Console.WriteLine(" context is null.");
        }

        try
        {
            // Convert the pre-zipped file URL into a BlobClient
            var blobClient = new BlobClient(new Uri(link));

            // Download the pre-zipped file as a stream
            var downloadInfo = await blobClient.DownloadAsync();
            var stream = downloadInfo.Value.Content;

            // Set the file name (you can modify this as needed)
            var fileName = $"Sermons_{title}.zip";

            // Return the stream for download
            //return File(stream, "application/zip", fileName);
            filesToCompress.Add((stream, "application/zip", fileName));
        }
        catch (Exception ex)
        {
            // Log the error for troubleshooting
            Console.WriteLine($"Error downloading pre-zipped file: {ex.Message}");
            
        }
    }

    public async Task DownloadSermon(int id, string? title, string? link)
    {
        if (id == null || _context.Sermons == null)
        {
            Console.WriteLine(" context is null.");
        }

        var sermon = await _context.Sermons
            .Include(s => s.MediaType)
            .Include(s => s.Minister)
            .Include(s => s.SermonType)
            .FirstOrDefaultAsync(s => s.SermonId == id);

        if (sermon == null)
        {
            Console.WriteLine(" context is null.");
        }

        // Fetch all blobs
        var availableBlobs = await FetchBlobs();
        if (availableBlobs == null || !availableBlobs.Any())
        {
            Console.WriteLine(" context is null.");
        }

        // Find the blob that matches the requested sermon
        BlobInfo sermonBlobInfo = availableBlobs
            .FirstOrDefault(blob =>
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(title);
                var relevantBlobNamePart = ExtractRelevantBlobName(fileNameWithoutExtension);

                return relevantBlobNamePart.Equals(sermon.Title, StringComparison.OrdinalIgnoreCase) ||
                       blob.FileName.Contains(sermon.DatePreached.ToString("dd.MM.yyyy"));
            });

        if (sermonBlobInfo == null)
        {
            Console.WriteLine(" context is null.");
        }

        try
        {
            // Convert URL string to Uri object
            var blobClient = new BlobClient(new Uri(link));

            // Download the blob as a stream
            var downloadInfo = await blobClient.DownloadAsync();
            var stream = downloadInfo.Value.Content;


            // Return the stream to the client for download without loading the file into memory
            //return File(stream, "application/octet-stream", sermonBlobInfo.FileName);
            
            filesToCompress.Add((stream, $"{title}.mp3", "audio/mpeg"));
        }
        catch (Exception ex)
        {
            // Log the error for troubleshooting
            Console.WriteLine($"Error downloading file: {ex.Message}");
            //return StatusCode(500, new { Message = "An error occurred while downloading the file." });
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

    //create checkout method to display all the items in the cartcthrough a viewmodel
    public IActionResult Checkout()
	{
		var cartItems = _cart.GetItems();
		var checkoutViewModel = new CheckoutViewModel
		{
			CartItems = cartItems
		};

		// Pass the total number of items in the cart to the view
		ViewBag.TotalItems = cartItems.Count;

		return View(checkoutViewModel);
	}




}
