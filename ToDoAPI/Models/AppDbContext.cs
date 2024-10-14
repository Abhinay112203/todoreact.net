using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToDoAPI.Models.ApplicationDbContext
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<ToDoList> Lists { get; set; }

        public DbSet<ListUser> ListUser { get; set; }
        public DbSet<Stage> Stages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Auto Generate
            modelBuilder.Entity<ToDoItem>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<ToDoList>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Stage>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            // Link Table, Index Table

            modelBuilder.Entity<ListUser>()
                .HasKey(ls => new { ls.ToDoListId, ls.UserId });

            // Entity relations one to many mutually exclusive,
            // Here list knows about stage and Stage knows about list.
            modelBuilder.Entity<Stage>()
            .HasOne(e => e.List)
            .WithMany(e => e.Stages)
            .HasForeignKey(e => e.ListId);

            modelBuilder.Entity<ToDoList>()
                .HasOne(m => m.CreatedBy)
                .WithMany(u => u.ToDoLists)
                .HasForeignKey(m => m.CreatedById);

            modelBuilder.Entity<ListUser>()
                .HasOne(e => e.User)
                .WithMany(e => e.ListUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<ListUser>()
                .HasOne(e => e.ToDoList)
                .WithMany(e => e.ListUsers)
                .HasForeignKey(e => e.ToDoListId);

            //entity relations one to many, one-way linked.
            // toDoItem knows about list, but list does not know about toDoItem.
            modelBuilder.Entity<ToDoItem>()
                .HasOne(e => e.ToDoList)
                .WithMany()
                .HasForeignKey(e => e.ListId);
        }
    }
}
