using Aspose.Slides;
using DCTStore.Data;
using DCTStore.Models;
using DCTStore.ViewModels;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net;
using DocumentFormat.OpenXml.Packaging;
using SixLabors.ImageSharp.Formats.Jpeg;
using Size = SixLabors.ImageSharp.Size;

namespace DCTStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _environment;

		public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, IWebHostEnvironment environment)
		{
			_context = context;
			_logger = logger;
			_environment = environment;
		}

        public IActionResult Index()
        {
			// Get the first 8 latest sermons excluding Krefeld Preaching
			var recentSermons = _context.Sermons
                .Include(s => s.MediaType)
                .Include(s => s.Minister)
                .Where(s => s.MediaType.MediaTypeId != 8)
                .OrderByDescending(s => s.DatePreached)
                .Take(8)
                .ToList();
			

			// Get the first 8 latest music excluding mediatypeid 5 and 6
			var recentMusic = _context.Musics
				.Include(m => m.MediaType)
				.Where(m => m.MediaType.MediaTypeId != 5 && m.MediaType.MediaTypeId != 6)
				.OrderByDescending(m => m.DateUploaded)
				.Take(8)
				.ToList();
			

			// get the first 8 lyrics 
			var recentLyrics = _context.Lyrics
				.Take(8)
				.ToList();

			
			
			// get the first 8 sermons where the sermon type is Krefeld Preaching
			var recentKrefeldSermons = _context.Sermons
                .Include(s => s.MediaType)
                .Include(s => s.Minister)
                .Where(s => s.MediaType.MediaTypeId == 8)
                .OrderByDescending(s => s.DatePreached)
                .Take(8)
                .ToList();

            // get any 8 music where the media typeID is equal to 5 and 6 only
			var recentKhuzimpiSheziPraiseAndWorship = _context.Musics
				.Include(m => m.MediaType)
				.Where(m => m.MediaType.MediaTypeId == 5 || m.MediaType.MediaTypeId == 6)
				.OrderByDescending(m => m.DateUploaded)
				.Take(8)
				.ToList();
			
			
            
            // You can create a ViewModel to hold both types of data if needed
            var viewModel = new HomeViewModel
			{
				RecentSermons = recentSermons,
				RecentMusic = recentMusic,
				RecentLyrics = recentLyrics,
                RecentKrefeldSermons = recentKrefeldSermons,
                RecentKhuzimpiSheziPraiseAndWorship = recentKhuzimpiSheziPraiseAndWorship,
				
				
            };

			return View(viewModel);
		}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}