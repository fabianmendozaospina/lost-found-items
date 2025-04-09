namespace LostAndFoundItems.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public ICollection<FoundItem> FoundItems { get; set; }
        public ICollection<LostItem> LostItems { get; set; }
    }
}
