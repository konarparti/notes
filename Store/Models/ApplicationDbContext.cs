using Microsoft.EntityFrameworkCore;

namespace Store.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductID = new Guid("63dc8fa6-07ae-4391-8916-e057f71239ce"),
                Name = "Kayak",
                Description = "A boat for one person",
                Category = "Watersport",
                Price = 5000
            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductID = new Guid("70bf165a-700a-4156-91c0-e83fce0a277f"),
                Name = "Football",
                Description = "A ball for paly footbal",
                Category = "Football",
                Price = 100
            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductID = new Guid("4aa76a4c-c59d-409a-84c1-06e6487a137a"),
                Name = "Soccer ball",
                Description = "A ball for paly soccer",
                Category = "Soccer",
                Price = 120.50m
            });
        }
    }
}
