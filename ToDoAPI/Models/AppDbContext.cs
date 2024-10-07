using Microsoft.EntityFrameworkCore;

namespace ToDoAPI.Models.ApplicationDbContext
{
    public partial class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<ToDoItem> ToDoItems { get; set; }  
        public DbSet<Users> Users { get; set; }
    }
}
