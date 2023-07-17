using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Expense_Tracker.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        #nullable disable
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        #nullable enable

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // need to call this for Identity

            modelBuilder
                .Entity<Transaction>()
                .Property(e => e.Date)
                .HasConversion(
                    v => v.ToUniversalTime(), // Convert to UTC when saving to DB
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)); // Specify as UTC when reading from DB
            
            modelBuilder
                .Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions) // specify the navigation property here
                .HasForeignKey(t => t.UserId)
                .IsRequired();
            
            modelBuilder
                .Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories) // specify the navigation property here
                .HasForeignKey(c => c.UserId)
                .IsRequired();
        }
    }
}
