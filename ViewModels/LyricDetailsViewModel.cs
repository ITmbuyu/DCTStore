using DCTStore.Models;

namespace DCTStore.ViewModels
{
    public class LyricDetailsViewModel
    {
        //create property to hold current lyric and a list of realted lyrics
        public Lyric CurrentLyric { get; set; }
        public List<Lyric> RelatedLyric { get; set; }
    }
}
