
using Microsoft.EntityFrameworkCore;
using GastroApp.Common;
using GastroApp.Domain.Entities;

namespace GastroApp.Infrastructure
{
    public class GastroAppDbContext : DbContext
    {
        public GastroAppDbContext(DbContextOptions<GastroAppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Discount> Discounts => Set<Discount>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Wspólne konwencje
            modelBuilder.Ignore<BaseEntity>();

            // Relacje
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.MenuItem)
                .WithMany()
                .HasForeignKey(i => i.MenuItemId);

            modelBuilder.Entity<Discount>()
                .HasIndex(d => d.Code)
                .IsUnique();

            // Dokladnosc dla cen
            modelBuilder.Entity<MenuItem>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<OrderItem>().Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Order>().Property(p => p.TotalAmount).HasColumnType("decimal(18,2)");
        }
    }
}