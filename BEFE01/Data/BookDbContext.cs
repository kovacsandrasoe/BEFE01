using BEFE01.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace BEFE01.Data
{
    public class BookDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Person> People { get; set; }

        public BookDbContext(DbContextOptions<BookDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne<Person>(t => t.Author)
                .WithMany(t => t.Books)
                .HasForeignKey(t => t.AuthorId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
