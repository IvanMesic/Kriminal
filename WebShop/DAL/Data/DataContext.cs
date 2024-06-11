using DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

/*
    -package manager console
    1. dotnet ef migrations add {migration name} --project WebShop
    2. dotnet ef database update --project WebShop
*/

namespace DAL.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DataContext(IConfiguration configuration, DbContextOptions<DataContext> options) : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("WebShop"));
        }

        //ORDER MATTERS
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Artist> Artist { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ArtistTag> ArtistTag { get; set; }
        public DbSet<ItemTag> ItemTag { get; set; }
        public DbSet<Bid> Bid { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<TransactionItem> TransactionItem { get; set; }


        //WRITE SPECIFIC CONSTRAINTS ON TABLES HERE
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<TransactionItem>()
            .Property(ti => ti.TransactionItemId)
            .ValueGeneratedOnAdd();
        }
    }
}
