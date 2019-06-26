using GenesisChallenge.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GenesisChallenge.DataAccess
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Telephone> Telephones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(b => b.Telephones)
                .WithOne();
        }
    }
}
