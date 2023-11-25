using DCTStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DCTStore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Minister> Ministers { get; set; }
        public DbSet<MininsterType> MininsterTypes { get; set; }
        public DbSet<Sermon> Sermons { get; set; }
        public DbSet<MediaType> MediaTypes { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<Lyric> Lyrics { get; set; }
        public DbSet<DCTStore.Models.SermonType> SermonType { get; set; } = default!;
    }
}