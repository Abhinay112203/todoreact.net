using Microsoft.AspNetCore.Identity;

namespace ToDoAPI.Models
{
    public class Users : IdentityUser
    {
        public DateTime? Dob { get; set; }
        public bool Deleted { get; set; } = false;
        public string? CreatedUserId { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? ModifiedUserId { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }

    public class CreateUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string? CreatedUserId { get; set; }

    }
}