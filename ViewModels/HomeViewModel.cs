using DCTStore.Models;

namespace DCTStore.ViewModels
{
	public class HomeViewModel
	{
		public List<Sermon> RecentSermons { get; set; }
		public List<SermonSubjects> SermonSubjects { get; set; }
		public List<Music> RecentMusic { get; set; }
		public List<Lyric> RecentLyrics { get; set; }
		public List<Sermon> RecentKrefeldSermons { get; set; }
		public List<Music> RecentKhuzimpiSheziPraiseAndWorship { get; set; }
		
	}

	public class SermonSubjects
	{
		public int SermonId { get; set; }
		public int SermonTypeId { get; set; }
		public string SermonType { get; set; }
		public MediaType MediaType { get; set; }
		public DateTime DatePreached { get; set; }
		public string MinisterName { get; set; }

	}
}
