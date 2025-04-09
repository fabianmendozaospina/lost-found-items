namespace LostAndFoundItems.Models
{
    public class MatchItem
    {
        public int MatchItemId { get; set; }
        public int FoundItemId { get; set; }
        public int LostItemId { get; set; }
        public int MatchUserId { get; set; }
        public int MatchStatusId { get; set; }
        public string Observation { get; set; }
        public FoundItem FoundItem { get; set; }
        public LostItem LostItem { get; set; }
        public User User { get; set; }
        public MatchStatus MatchStatus { get; set; }
    }
}
