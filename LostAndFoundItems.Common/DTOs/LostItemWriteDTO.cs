namespace LostAndFoundItems.Common.DTOs
{
    public class LostItemWriteDTO
    {
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime LostDate { get; set; }
    }
}
