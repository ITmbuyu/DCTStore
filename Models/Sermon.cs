namespace DCTStore.Models
{
    public class Sermon
    {
        public int SermonId { get; set; }
        public string Title { get; set; }
        public DateTime DatePreached { get; set; }
        public int? MinisterId { get; set; }
        public virtual Minister? Minister { get; set; }
        public string SermonMediaLink { get; set; }
        public string SermonVideoLink { get; set; }
        public int? SermonTypeId { get; set; }
        public virtual SermonType? SermonType { get; set; }
        public int? MediaTypeId { get; set; }
        public virtual MediaType? MediaType { get; set; }
    }
}