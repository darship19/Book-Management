using BookApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Data
{
    public class BookDBContext : DbContext
    {
        public BookDBContext(DbContextOptions<BookDBContext> options) : base(options)
        {
        }

        // DbSet for the Book entity
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Book entity
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id); // Primary key

                entity.Property(b => b.Id)
                    .ValueGeneratedOnAdd();  // Ensures the ID is auto-generated (if not already default behavior)

                entity.Property(b => b.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(b => b.Author)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(b => b.ISBN)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(b => b.PublicationDate)
                    .IsRequired();
            });
        }

    }
}
