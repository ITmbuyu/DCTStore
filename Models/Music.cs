namespace DCTStore.Models
{
    public class Music
    {
        public int MusicId { get; set; }
        public string Title { get; set; }
        public string MusicMediaLink { get; set; }
        public int? LyricId { get; set; }
        public virtual Lyric? Lyric { get; set; }
        public int? MediaTypeId { get; set; }
        public virtual MediaType? MediaType { get; set; }
        public DateTime DateUploaded { get; set; }

    }
}