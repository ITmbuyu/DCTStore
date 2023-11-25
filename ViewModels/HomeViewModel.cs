using DCTStore.Models;

namespace DCTStore.ViewModels
{
	public class HomeViewModel
	{
		public List<Sermon> RecentSermons { get; set; }
		public List<Music> RecentMusic { get; set; }
		public List<Lyric> RecentLyrics { get; set; }
		public List<Sermon> RecentKrefeldSermons { get; set; }
		public List<Music> RecentKhuzimpiSheziPraiseAndWorship { get; set; }
		
	}
}
