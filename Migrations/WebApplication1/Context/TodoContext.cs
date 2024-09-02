using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using WebApplication1.Models;

namespace WebApplication1.Context
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique index on username
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Configure Todo entity
            modelBuilder.Entity<Todo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.TargetDate).HasColumnType("date");
                entity.Property(e => e.Status).HasDefaultValue(false); // Default value for status
            });

            // Seeding Data (Uncomment if needed)
            //modelBuilder.Entity<User>().HasData(
            //    new User { Id = 1, FirstName = "John", LastName = "Doe", Username = "johndoe", Password = "password" },
            //    new User { Id = 2, FirstName = "Jane", LastName = "Doe", Username = "janedoe", Password = "password" }
            //);

            //modelBuilder.Entity<Todo>().HasData(
            //    new Todo { Id = 1, Title = "Learn ASP.NET Core", Username = "johndoe", Description = "Learn how to build web APIs with ASP.NET Core.", TargetDate = DateTime.Now.AddDays(7), Status = false },
            //    new Todo { Id = 2, Title = "Build Todo App", Username = "janedoe", Description = "Build a Todo app to manage tasks.", TargetDate = DateTime.Now.AddDays(14), Status = false }
            //);
        }
    }
}
