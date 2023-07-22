using Microsoft.EntityFrameworkCore;
using CCTest.Database.Entities;

namespace CCTest.Database.Common
{
    public class CCTestDbContext : DbContext
    {
        public CCTestDbContext(DbContextOptions<CCTestDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<Team> Teams { get; set; }
    }
}
