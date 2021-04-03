using Microsoft.EntityFrameworkCore;
using Vaccinator.Models;

namespace Vaccinator.Controllers
{
    public class ContextBDD : DbContext
    {
        public DbSet<Injection> Injections { get; set; }
        public DbSet<Personne> Personnes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("Host=localhost,Database=vaccinator,Username=baptistebrs;Password=442306200007,");
            optionsBuilder.UseSqlite("Data Source = vaccinator.db");
            //optionsBuilder.UseNpgsql("data source = vaccinator.db");

        }
    }
}