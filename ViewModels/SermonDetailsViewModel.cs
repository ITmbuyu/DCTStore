using DCTStore.Models;

namespace DCTStore.ViewModels
{
	public class SermonDetailsViewModel
	{
		// create a property to hold the current sermon
		public Sermon CurrentSermon { get; set; }
		public List<Sermon> RelatedSermon { get; set; }
	}
}
