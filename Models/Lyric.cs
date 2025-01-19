namespace DCTStore.Models
{
    public class Lyric
    {
        public int LyricId { get; set; }
        public string Title { get; set; }
        public string LyricMediaLink { get; set; }
        public string LyricThumbnail { get; set; }
        public int? LyricTypeId { get; set; }
        public virtual LyricType? LyricType { get; set; }
    }
}