namespace LostAndFoundItems.Models
{
    public class MatchStatus
    {
        public int MatchStatusId { get; set; }
        public string Name { get; set; }
        public ICollection<MatchItem> MatchItems { get; set; }
    }
}
