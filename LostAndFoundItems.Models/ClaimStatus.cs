namespace LostAndFoundItems.Models
{
    public class ClaimStatus
    {
        public int ClaimStatusId { get; set; }
        public string Name { get; set; }
        public ICollection<ClaimRequest> ClaimRequests { get; set; }
    }
}