using System.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using OwlAnalysis.Model;
using System.Collections.Specialized;


namespace OwlAnalysis.Dao
{
    public class OwlAnalysisContex : DbContext
    {
        
        public OwlAnalysisContex()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                //.UseLazyLoadingProxies()
                .UseSqlServer(ConfigurationManager.ConnectionStrings["OwlAnalysisDatabase"].ConnectionString);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>()
                .HasMany(m => m.HomeMatches)
                .WithOne(t => t.HomeTeam);

            modelBuilder.Entity<Team>()
                .HasMany(m => m.AwayMatches)
                .WithOne(t => t.AwayTeam);
                
        }

        public DbSet<Stage> Stages { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerGame> PlayerGames { get; set; }
        public DbSet<PlayerHeroStats> PlayerHeroStats { get; set; }
    }

}