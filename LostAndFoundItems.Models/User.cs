namespace LostAndFoundItems.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public ICollection<ClaimRequest> ClaimRequests { get; set; }
        public ICollection<LostItem> LostItems { get; set; }
        public ICollection<MatchItem> MatchItems { get; set; }
    }
}