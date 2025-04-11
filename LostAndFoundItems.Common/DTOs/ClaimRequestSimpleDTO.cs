namespace LostAndFoundItems.Common.DTOs
{
    public class ClaimRequestSimpleDTO
    {
        public int ClaimRequestId { get; set; }
        public int ClaimingUserId { get; set; }
        public string ClaimingUserFullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ClaimStatusId { get; set; }
        public string ClaimStatusName { get; set; }
        public string Message { get; set; }
    }
}