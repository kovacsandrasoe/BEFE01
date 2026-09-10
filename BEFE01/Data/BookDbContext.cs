using BEFE01.Models;
using Microsoft.EntityFrameworkCore;

namespace BEFE01.Data
{
    public class BookDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Person> People { get; set; }

        public BookDbContext(DbContextOptions<BookDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
