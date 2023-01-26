using Newtonsoft.Json;
using Task2.Data;
using Task2.Models;

namespace Task2.Services
{
    public class BookService
    {
        private readonly BookContext _context;
        private readonly string _secretKey = string.Empty;

        public BookService(BookContext context)
        {
            ConfigurationBuilder configurationBuilder = new();

            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("appsettings.json");

            _secretKey = configurationBuilder.Build().GetValue<string>("AppSettings:SecretKey");

            _context = context;
            if (_context.Database.EnsureCreated() || !_context.Books.Any())
            {
                DbInitializer.Initialize(_context);
            }
        }

        public async Task<IEnumerable<BookDTO>?> GetTopBooksAsync(string? genre)
        {
            IEnumerable<BookDTO>? result = null;

            await Task.Run(() =>
            {
                Console.Write("GET https://localhost:5000/api/recommended");

                if (_context.Books == null || _context.Books.Count() == 0)
                {
                    return;
                }

                var books = _context.Books.AsEnumerable();

                if (genre != null)
                {
                    books = books.Where(book => book.Genre?.ToLower() == genre.ToLower());
                    Console.WriteLine($"?genre={genre}");
                }
                else
                {
                    Console.WriteLine();
                }

                if (books == null)
                {
                    return;
                }

                result = books
                    .OrderByDescending(book =>
                        book.Ratings == null ? 0 :
                        book.Ratings.Count() == 0 ? 0 :
                        book.Ratings.Average(rating => rating.Score))
                    .Take(10)
                    .Select(book => new BookDTO
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Author = book.Author,
                        AverageRatingsScore = book.Ratings == null
                            ? 0
                            : book.Ratings.Count() == 0
                                ? 0
                                : (decimal)book.Ratings.Average(rating => rating.Score),
                        ReviewsCount = book.Reviews == null
                            ? 0
                            : book.Reviews.Count()
                    });
            });

            return result;
        }

        public async Task<IEnumerable<BookDTO>?> GetAllBooksAsync(string? order)
        {
            IEnumerable<BookDTO> result = Array.Empty<BookDTO>();

            await Task.Run(() =>
            {
                Console.Write("GET https://localhost:5000/api/books");

                if (order != null)
                {
                    Console.WriteLine($"?order={order}");
                }
                else
                {
                    Console.WriteLine();
                }

                if (_context.Books == null || _context.Books.Count() == 0)
                {
                    result = Array.Empty<BookDTO>();
                    return;
                }

                result = _context.Books.Select(book => new BookDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    AverageRatingsScore = book.Ratings == null
                        ? 0
                        : book.Ratings.Count() == 0
                            ? 0
                            : (decimal)book.Ratings.Average(rating => rating.Score),
                    ReviewsCount = book.Reviews == null
                        ? 0
                        : book.Reviews.Count()
                });
            });

            return order == null
                ? result
                : order.ToLower() == "author"
                    ? result.OrderBy(book => book.Author)
                    : order.ToLower() == "title"
                        ? result.OrderBy(book => book.Title)
                        : null;
        }

        public async Task<BookContentDTO?> GetBookByIdAsync(int id)
        {
            Console.WriteLine($"GET https://localhost:5000/api/books/{id}");

            BookDetailsDTO? book = await _context.Books.FindAsync(id);

            return book == null
                ? null
                : new BookContentDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Cover = book.Cover ?? string.Empty,
                    Content = book.Content,
                    AverageRatingsScore = book.Ratings == null
                        ? 0
                        : book.Ratings.Count() == 0
                            ? 0
                            : (decimal)book.Ratings.Average(book => book.Score),
                    Reviews = (book.Reviews == null || book.Reviews.Count() == 0)
                        ? Array.Empty<ReviewDTO>()
                        : book.Reviews.Select(review => new ReviewDTO
                        {
                            Id = review.Id,
                            Message = review.Message,
                            Reviewer = review.Reviewer
                        })
                        .ToArray()
                };
        }

        public async Task<int?> PostAsync(BookPutDTO book)
        {
            int? id = null;
            await Task.Run(() =>
            {
                Console.WriteLine("POST https://localhost:5000/api/books/save");
                Console.WriteLine(JsonConvert.SerializeObject(book, Formatting.Indented));

                if (book == null || book.Title == null || book.Author == null || book.Content == null || book.Genre == null)
                {
                    return;
                }

                BookDetailsDTO newBook = new()
                {
                    Title = book.Title,
                    Author = book.Author,
                    Cover = book.Cover,
                    Content = book.Content,
                    Genre = book.Genre
                };

                if (book.Id == null)
                {
                    _context.Books.Add(newBook);
                    id = _context.Books.ToList().Last().Id + 1; //without SaveChanges element not in db
                }
                else
                {
                    BookDetailsDTO? foundBook = _context.Books.Find(book.Id);

                    if (foundBook == null)
                    {
                        _context.Books.Add(newBook);
                        _context.SaveChanges();

                        id = _context.Books.ToList().Last().Id;
                    }
                    else
                    {
                        foundBook.Title = book.Title;
                        foundBook.Author = book.Author;
                        foundBook.Cover = book.Cover;
                        foundBook.Content = book.Content;
                        foundBook.Genre = book.Genre;

                        id = foundBook.Id;
                    }

                }

                _context.SaveChanges();
            });

            return id;
        }

        public async Task<int?> PutReviewAsync(int id, ReviewDTO review)
        {
            int? result = null;

            await Task.Run(() =>
            {
                Console.WriteLine($"PUT https://localhost:5000/api/books/{id}/review");
                Console.WriteLine(JsonConvert.SerializeObject(review, Formatting.Indented));

                if (review == null || review.Message == null || review.Reviewer == null)
                {
                    return;
                }

                BookDetailsDTO? book = _context.Books.Find(id);

                if (book == null)
                {
                    return;
                }

                book.Reviews.Add(new ReviewDetailsDTO
                {
                    Message = review.Message,
                    Reviewer = review.Reviewer
                });

                _context.SaveChanges();
                result = book.Reviews.ToList().Last().Id;
            });

            return result;
        }

        public async Task<int?> PutRatingAsync(int id, RatingDTO rating)
        {
            int? result = null;

            await Task.Run(() =>
            {
                Console.WriteLine($"PUT https://localhost:5000/api/books/{id}/rating");
                Console.WriteLine(JsonConvert.SerializeObject(rating, Formatting.Indented));

                if (rating == null || rating.Score == null || rating.Score < 1 || rating.Score > 5)
                {
                    return;
                }

                BookDetailsDTO? book = _context.Books.Find(id);

                if (book == null)
                {
                    return;
                }

                book.Ratings.Add(new RatingDetailsDTO
                {
                    Score = rating.Score
                });

                _context.SaveChanges();

                result = book.Ratings.ToList().Last().Id;
            });

            return result;
        }

        public async Task<BookDetailsDTO?> DeleteAsync(int id, string secretKey)
        {
            BookDetailsDTO? book = null;

            await Task.Run(() =>
            {
                Console.Write($"DELETE https://localhost:5000/api/books/{id}");

                if (secretKey != null)
                {
                    Console.WriteLine($"?secretkey={secretKey}");
                }
                else
                {
                    Console.WriteLine();
                }

                if (secretKey == null || secretKey != _secretKey)
                {
                    return;
                }

                if (_context.Books == null || _context.Books.Count() == 0)
                {
                    return;
                }

                book = _context.Books.Find(id);

                if (book == null)
                {
                    return;
                }

                _context.Books.Remove(book);
                _context.SaveChanges();
            });

            return book;
        }
    }
}