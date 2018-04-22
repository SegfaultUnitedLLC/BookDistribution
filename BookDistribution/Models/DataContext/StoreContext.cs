using Microsoft.EntityFrameworkCore;

namespace BookDistribution.Models
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }
        public DbSet<Store> Store { get; set; }
    }
}
