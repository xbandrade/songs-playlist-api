using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GlobalPlaylist.Data;
public class PlaylistDBContext : DbContext
{
    public DbSet<Song> Songs { get; set; }
    private readonly IConfiguration _configuration;

    public PlaylistDBContext(IConfiguration configuration, DbContextOptions<PlaylistDBContext> options) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {    
        string? connectionString = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), null);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}