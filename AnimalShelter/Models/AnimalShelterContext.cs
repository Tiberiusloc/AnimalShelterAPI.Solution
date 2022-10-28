using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AnimalShelter.Models
{
    public class AnimalShelterContext : IdentityDbContext
    {
        public AnimalShelterContext(DbContextOptions<AnimalShelterContext> options)
            : base(options)
        {
          
        }
        public DbSet<Animal> Animals { get; set; }
    }
}