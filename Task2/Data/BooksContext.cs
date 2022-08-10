using Microsoft.EntityFrameworkCore;
using Task2.Data.DataModels;

namespace Task2.Data
{
    public class BooksContext : DbContext
    {
        public DbSet<BookDetailDTO> Books { get; set; }
        public DbSet<RatingDetailsDTO> Ratings { get; set; }
        public DbSet<ReviewDetailsDTO> Reviews { get; set; }

        public BooksContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}