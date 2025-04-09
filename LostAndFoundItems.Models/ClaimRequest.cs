namespace LostAndFoundItems.Models
{
    public class ClaimRequest
    {
        public int ClaimRequestId { get; set; }
        public int FoundItemId { get; set; }
        public int ClaimingUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ClaimStatusId { get; set; }
        public string Message { get; set; }
        public ClaimStatus ClaimStatus { get; set; }
        public User User { get; set; }
        public FoundItem FoundItem { get; set; }

    }
}
