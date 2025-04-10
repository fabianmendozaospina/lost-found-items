using System.Data;

namespace LostAndFoundItems.Common.DTOs {
    public class UserDTO {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public RoleDTO Role { get; set; }
    }
}
