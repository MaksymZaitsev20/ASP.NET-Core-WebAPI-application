using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Task2.Data.DataModels;

namespace Task2.Data
{
    public static class DbManager
    {
        private static SqliteConnection connection;
        public static BooksContext Context { get; private set; }
        public static string SecretKey { get; private set; }

        static DbManager()
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("appsettings.json");

            connection = new SqliteConnection(
                configurationBuilder.Build()
                .GetConnectionString("BooksContext"));

            SecretKey = configurationBuilder.Build().GetValue<string>("AppSettings:SecretKey");

            connection.Open();

            Context = new BooksContext(
                new DbContextOptionsBuilder<BooksContext>()
                .UseSqlite(connection)
                .Options);

            if (Context.Database.EnsureCreated())
                DbInitializer.Initialize(Context);
        }

        public static async Task<IEnumerable<BookDTO>?> GetTopBooksAsync(string? genre)
        {
            IEnumerable<BookDTO> result = null;

            await Task.Run(() =>
            {
                Console.Write("GET https://localhost:5000/api/recommended");

                if (Context.Books == null || Context.Books.Count() == 0)
                    return;

                var books = Context.Books.AsEnumerable();

                if (genre != null)
                {
                    books = books.Where(book => book.Genre.ToLower() == genre.ToLower());
                    Console.WriteLine($"?genre={genre}");
                }
                else
                    Console.WriteLine();

                if (books == null)
                    return;

                result = books.OrderByDescending(
                    book =>
                        book.Ratings == null ? 0 :
                        book.Ratings.Count() == 0 ? 0 :
                        book.Ratings.Average(rating => rating.Score))
                      .Take(10).Select(
                          book =>
                              new BookDTO
                              {
                                  Id = book.Id,
                                  Title = book.Title,
                                  Author = book.Author,
                                  AverageRatingsScore =
                                      book.Ratings == null ? 0 :
                                      book.Ratings.Count() == 0 ? 0 :
                                      (decimal)book.Ratings.Average(rating => rating.Score),
                                  ReviewsCount =
                                      book.Reviews == null ? 0 : book.Reviews.Count()
                              });
            });

            return result;
        }
        public static async Task<IEnumerable<BookDTO>> GetAllBooksAsync(string? order)
        {
            IEnumerable<BookDTO> result = Array.Empty<BookDTO>();
            
            await Task.Run(() =>
            {
                Console.Write("GET https://localhost:5000/api/books");
                if (order != null)
                    Console.WriteLine($"?order={order}");
                else
                    Console.WriteLine();

                if (Context.Books == null || Context.Books.Count() == 0)
                {
                    result = Array.Empty<BookDTO>();
                    return;
                }

                result = Context.Books.Select(
                    book =>
                        new BookDTO
                        {
                            Id = book.Id,
                            Title = book.Title,
                            Author = book.Author,
                            AverageRatingsScore =
                                book.Ratings == null ? 0 :
                                book.Ratings.Count() == 0 ? 0 :
                                (decimal)book.Ratings.Average(rating => rating.Score),
                            ReviewsCount =
                                book.Reviews == null ? 0 : book.Reviews.Count()
                        });
            });
            
            return order == null ? result :
                order.ToLower() == "author" ? result.OrderBy(book => book.Author) :
                order.ToLower() == "title" ? result.OrderBy(book => book.Title) :
                null;
        }
        public static async Task<BookContentDTO> GetBookByIdAsync(int id)
        {
            Console.WriteLine($"GET https://localhost:5000/api/books/{id}");

            var book = await Context.Books.FindAsync(id);

            if (book == null)
                return null;

            return new BookContentDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Cover = book.Cover ?? String.Empty,
                Content = book.Content,
                AverageRatingsScore =
                    book.Ratings == null ? 0 :
                    book.Ratings.Count() == 0 ? 0 :
                    (decimal)book.Ratings.Average(book => book.Score),
                Reviews =
                    (book.Reviews == null || book.Reviews.Count() == 0) ? Array.Empty<ReviewDTO>() :
                    book.Reviews.Select(
                    review =>
                        new ReviewDTO
                        {
                            Id = review.Id,
                            Message = review.Message,
                            Reviewer = review.Reviewer
                        }).ToArray()
            };
        }
        public static async Task<int?> PostAsync(BookPutDTO book)
        {
            int? id = null;
            await Task.Run(() =>
            {
                Console.WriteLine("POST https://localhost:5000/api/books/save");
                Console.WriteLine(JsonConvert.SerializeObject(book, Formatting.Indented));

                if (book == null || book.Title == null || book.Author == null || book.Content == null || book.Genre == null)
                    return;

                var newBook = new BookDetailDTO
                {
                    Title = book.Title,
                    Author = book.Author,
                    Cover = book.Cover,
                    Content = book.Content,
                    Genre = book.Genre
                };

                if (book.Id == null)
                {
                    Context.Books.Add(newBook);
                    id = Context.Books.ToList().Last().Id + 1; //without SaveChanges element not in db
                }
                else
                {
                    var foundBook = Context.Books.Find(book.Id);

                    if (foundBook == null)
                    {
                        Context.Books.Add(newBook);
                        Context.SaveChanges();

                        id = Context.Books.ToList().Last().Id;
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

                Context.SaveChanges();
            });

            return id;
        }
        public static async Task<int?> PutReviewAsync(int id, ReviewDTO review)
        {
            int? result = null;

            await Task.Run(() =>
            {
                Console.WriteLine($"PUT https://localhost:5000/api/books/{id}/review");
                Console.WriteLine(JsonConvert.SerializeObject(review, Formatting.Indented));

                if (review == null || review.Message == null || review.Reviewer == null)
                    return;

                var book = Context.Books.Find(id);

                if (book == null)
                    return;

                book.Reviews.Add(new ReviewDetailsDTO
                {
                    Message = review.Message,
                    Reviewer = review.Reviewer
                });

                Context.SaveChanges();
                result = book.Reviews.ToList().Last().Id;
            });

            return result;
        }
        public static async Task<int?> PutRatingAsync(int id, RatingDTO rating)
        {
            int? result = null;

            await Task.Run(() =>
            {
                Console.WriteLine($"PUT https://localhost:5000/api/books/{id}/rating");
                Console.WriteLine(JsonConvert.SerializeObject(rating, Formatting.Indented));

                if (rating == null || rating.Score == null || rating.Score < 1 || rating.Score > 5)
                    return;

                var book = Context.Books.Find(id);

                if (book == null)
                    return;

                book.Ratings.Add(new RatingDetailsDTO
                {
                    Score = rating.Score
                });

                Context.SaveChanges();

                result = book.Ratings.ToList().Last().Id;
            });

            return result;
        }
        public static async Task<BookDetailDTO?> Delete(int id, string secretKey)
        {
            BookDetailDTO? book = null;

            await Task.Run(() =>
            {
                Console.Write($"DELETE https://localhost:5000/api/books/{id}");
                if (secretKey != null)
                    Console.WriteLine($"?secretkey={secretKey}");
                else
                    Console.WriteLine();

                if (secretKey == null || secretKey != SecretKey)
                    return;

                if (Context.Books == null || Context.Books.Count() == 0)
                    return;

                book = Context.Books.Find(id);

                if (book == null)
                    return;

                Context.Books.Remove(book);
                Context.SaveChanges();
            });

            return book;
        }
    }
}
