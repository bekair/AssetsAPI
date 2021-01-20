using AssetsAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetsAPI.DataAccess
{
    public class AssetsDbContext : DbContext
    {
        public AssetsDbContext(DbContextOptions options)
            : base(options)
        { }

        public AssetsDbContext()
            : base()
        { }

        public DbSet<Asset> Assets { get; set; }
    }
}
