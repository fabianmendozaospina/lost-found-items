namespace LostAndFoundItems.Common.DTOs
{
    public class FoundItemDTO
    {
        public int FoundItemId { get; set; }
        public int UserId { get; set; }
        public string? UserFullName { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime FoundDate { get; set; }
        public ICollection<ClaimRequestSimpleDTO> ClaimRequests { get; set; }
    }
}
