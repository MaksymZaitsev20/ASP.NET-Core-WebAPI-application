using Microsoft.EntityFrameworkCore;
using Task2.Models;

namespace Task2.Data
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<BookDetailsDTO> Books => Set<BookDetailsDTO>();
        public DbSet<RatingDetailsDTO> Ratings => Set<RatingDetailsDTO>();
        public DbSet<ReviewDetailsDTO> Reviews => Set<ReviewDetailsDTO>();
    }
}