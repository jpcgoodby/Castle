using Microsoft.EntityFrameworkCore;

namespace Castle.Data
{
    public class CastleContext : DbContext
    {
        public CastleContext(DbContextOptions<CastleContext> options)
            : base(options) { }

        public DbSet<Castle> Castles { get; set; }
    }
}
