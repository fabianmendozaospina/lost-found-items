using LostAndFoundItems.Models;

namespace LostAndFoundItems.Common.DTOs
{
    public class MatchItemDTO
    {
        public int MatchItemId { get; set; }
        public int FoundItemId { get; set; }
        public string FoundItemTitle { get; set; }
        public int LostItemId { get; set; }
        public string LostItemIdTitle { get; set; }
        public int MatchUserId { get; set; }
        public string MatchUserName { get; set; }
        public int MatchStatusId { get; set; }
        public string MatchStatusName { get; set; }
        public string Observation { get; set; }
    }
}
