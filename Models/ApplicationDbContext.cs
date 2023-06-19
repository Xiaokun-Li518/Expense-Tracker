using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        #nullable disable
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        #nullable enable

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Transaction>()
                .Property(e => e.Date)
                .HasConversion(
                    v => v.ToUniversalTime(), // Convert to UTC when saving to DB
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)); // Specify as UTC when reading from DB
        }

    }
}
