using HarshitCommunications.Models;
using Microsoft.EntityFrameworkCore;

namespace HarshitCommunications.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category {Id = 1, Name = "Charger", DisplayOrder = 1 },
                new Category {Id = 2, Name = "Cables", DisplayOrder = 2 },
                new Category {Id = 3, Name = "Air Buds", DisplayOrder = 3 },
                new Category {Id = 4, Name = "Neck Band", DisplayOrder = 4 },
                new Category {Id = 5, Name = "Car Charger", DisplayOrder = 5 },
                new Category {Id = 6, Name = "Power Bank", DisplayOrder = 6 },
                new Category {Id = 7, Name = "Smart Watch", DisplayOrder = 7 },
                new Category {Id = 8, Name = "Speakers", DisplayOrder = 8 },
                new Category {Id = 9, Name = "Earphone ", DisplayOrder = 9 }
                );
        }
    }
}