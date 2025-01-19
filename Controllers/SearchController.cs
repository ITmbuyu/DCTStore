using DCTStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DCTStore.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

		public IActionResult Index(string searchTerm, int page = 1, int pageSize = 10)
		{
			if (string.IsNullOrWhiteSpace(searchTerm))
			{
				return View(); // Display the search box
			}

			var searchResults = _context.Lyrics
				.Where(l => l.Title.Contains(searchTerm))
				.Include(l => l.LyricType)
				.Cast<object>()
				.ToList();

			var sermonResults = _context.Sermons
				.Where(s => s.Title.Contains(searchTerm))
				.Include(s => s.MediaType)
				.Include(s => s.Minister)
				.Include(s => s.SermonType)
				.Cast<object>()
				.ToList();

			var musicResults = _context.Musics
				.Where(m => m.Title.Contains(searchTerm))
				.Include(m => m.MediaType)
				.Include(m => m.Lyric)
				.Cast<object>()
				.ToList();

			var allResults = searchResults.Concat(sermonResults).Concat(musicResults);

			ViewBag.SearchTerm = searchTerm;

			// Paginate the results
			var paginatedResults = allResults.Skip((page - 1) * pageSize).Take(pageSize);

			// Return a partial view with the paginated search results
			return PartialView("_SearchResultsPartial", paginatedResults);
		}

	}


}
