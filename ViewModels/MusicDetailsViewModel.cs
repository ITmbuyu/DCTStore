using DCTStore.Models;

namespace DCTStore.ViewModels
{
	public class MusicDetailsViewModel
	{
		// create a property to hold the current music
		public Music CurrentMusic { get; set; }
		public List<Music> RelatedMusic { get; set; }
	}
}
