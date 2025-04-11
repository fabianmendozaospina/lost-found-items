namespace LostAndFoundItems.Common.DTOs
{
    public class ClaimRequestWriteDTO
    {
        public int FoundItemId { get; set; }
        public int ClaimingUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ClaimStatusId { get; set; }
        public string Message { get; set; }
    }
}
