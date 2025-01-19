namespace DCTStore.Models
{
    public class SermonType
    {
        public int SermonTypeId { get; set; }
        public string Type { get; set; }
        public int ? MinisterId { get; set; }
        public virtual Minister? Minister { get; set; }
        public string TypeDownloadLink { get; set; }
    }
}
