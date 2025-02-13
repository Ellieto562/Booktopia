using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Booktopia.Models.Entities;

namespace Booktopia.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<ReadingList> ReadingLists { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReadingList>()
                .HasKey(rl => new { rl.UserId, rl.BookId });

            modelBuilder.Entity<ReadingList>()
           .HasOne(rl => rl.Book)
           .WithMany(b => b.ReadingList)
           .HasForeignKey(rl => rl.BookId);

            modelBuilder.Entity<ReadingList>()
                .HasOne(rl => rl.User)
                .WithMany()
                .HasForeignKey(rl => rl.UserId);
        }
    }
}
