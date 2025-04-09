namespace LostAndFoundItems.Models
{
    public class FoundItem
    {
        public int FoundItemId { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime FoundDate { get; set; }
        public Category Category { get; set; }
        public Location Location { get; set; }
        public User User { get; set; }
        public MatchItem MatchItem { get; set; }
        public ICollection<ClaimRequest> ClaimRequests { get; set; }
    }
}