
namespace IRunesWebApp.Data
{
    using IRunesWebApp.Models;
    using Microsoft.EntityFrameworkCore;

    public class IRunesContext : DbContext
    {
        public IRunesContext()
        {
        }
        public IRunesContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Album> Albums{ get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<TrackAblum> TrackAblums{ get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(Configuration.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TrackAblum>()
                .HasKey(ta => new { ta.AlbumId, ta.TrackId });

            base.OnModelCreating(builder);
        }
    }
}
