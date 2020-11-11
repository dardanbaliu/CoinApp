using CoinApp.Domain.Coins;
using CoinApp.Domain.Users;
using CoinApp.Infrastructure.Coins;
using CoinApp.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CoinApp.Infrastructure
{
    public class CoinAppContext : DbContext
    {
        //DBO schema
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Coin> Coins { get; set; }

        public CoinAppContext(DbContextOptions options) : base(options)
        {

        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            return base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //db config
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CoinEntityTypeConfiguration());
        }
    }
}
