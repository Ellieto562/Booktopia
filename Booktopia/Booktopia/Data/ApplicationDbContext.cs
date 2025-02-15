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
        public DbSet<BookReadingList> BookReadingLists { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<BookReadingList>()
               .HasOne(brl => brl.Book)
               .WithMany(b => b.BookReadingLists)
               .HasForeignKey(brl => brl.BookId);

            modelBuilder.Entity<BookReadingList>()
                .HasOne(brl => brl.ReadingList)
                .WithMany(rl => rl.BookReadingLists)
                .HasForeignKey(brl => brl.ReadingListId);

        }
    }
}
