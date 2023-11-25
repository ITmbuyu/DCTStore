namespace DCTStore.Models
{
    public class Minister
    {
        public int MinisterId { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        

        // Foreign key
        public int? MininsterTypeId { get; set; }
        // Navigation property
        public virtual MininsterType ? MininsterType { get; set; }
    }
}