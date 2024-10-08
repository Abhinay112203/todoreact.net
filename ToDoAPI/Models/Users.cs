using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

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

        public virtual ICollection<ToDoList>? ToDoLists { get; set; }

        public virtual ICollection<ListUser> ListUsers { get; set; }
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

    public class UserResponse{
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string? token { get; set; }
    }
}