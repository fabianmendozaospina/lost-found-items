namespace LostAndFoundItems.Common.DTOs
{
    public class MatchItemWriteDTO
    {
        public int FoundItemId { get; set; }
        public int LostItemId { get; set; }
        public int MatchUserId { get; set; }
        public int MatchStatusId { get; set; }
        public string Observation { get; set; }
    }
}
