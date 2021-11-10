using Microsoft.EntityFrameworkCore;
using Back.Models;

namespace Back
{
    public class AppDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

        public AppDBContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql("Host=localhost;Port=5432;Database=ais;Username=postgres;Password=123qwe");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
        }
    }
}
