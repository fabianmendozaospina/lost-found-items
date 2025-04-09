namespace LostAndFoundItems.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<FoundItem> FoundItems { get; set; }
        public ICollection<LostItem> LostItems { get; set; }
    }
}
