namespace DCTStore.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemCategory { get; set; }
        public string Title { get; set; }
        public string MediaLink { get; set; }
        public string? VideoMediaLink { get; set; }
        public string imagemedia { get; set; }
    }
}
