using Microsoft.AspNetCore.Identity;

namespace ToDoAPI.Models
{
    public partial class UserRole : IdentityRole
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public byte RoleId { get; set; }
        public int? Slaid { get; set; }
        public bool? Deleted { get; set; }
    }
}
