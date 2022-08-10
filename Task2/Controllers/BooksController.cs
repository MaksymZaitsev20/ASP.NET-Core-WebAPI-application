using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Task2.Data;
using Task2.Data.DataModels;

namespace Task2.Controllers
{
    [Route("api")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;

        public BooksController(ILogger<BooksController> logger)
            => _logger = logger;

        [HttpGet("recommended")]
        public async Task<ActionResult<BookDTO>> GetBooksTop(string? genre)
        {
            Console.Write("GET https://localhost:5000/api/recommended");

            if (DbManager.Context.Books == null || DbManager.Context.Books.Count() == 0)
                return NoContent();

            var books = DbManager.Context.Books.AsEnumerable();

            if (genre != null)
            {
                books = books.Where(book => book.Genre.ToLower() == genre.ToLower());
                Console.WriteLine($"?genre={genre}");
            }
            else
                Console.WriteLine();

            if (books == null)
                return NoContent();

            var result = books.OrderByDescending(
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

            return new OkObjectResult(result);
        }

        // GET: api/books?order=author
        [HttpGet("books")]
        public async Task<ActionResult<BookDTO>> GetAll(string? order = null)
        {
            Console.Write("GET https://localhost:5000/api/books");
            if (order != null)
                Console.WriteLine($"?order={order}");
            else
                Console.WriteLine();

            if (DbManager.Context.Books == null || DbManager.Context.Books.Count() == 0)
                return NoContent();

            var result = DbManager.Context.Books.Select(
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

            return order == null ? new OkObjectResult(result) :
                order.ToLower() == "author" ? new OkObjectResult(result.OrderBy(book => book.Author)) :
                order.ToLower() == "title" ? new OkObjectResult(result.OrderBy(book => book.Title)) :
                new BadRequestResult();
        }

        // GET api/<BooksController>/5
        [HttpGet("books/{id}")]
        public async Task<ActionResult<BookContentDTO>> GetById(int id)
        {
            Console.WriteLine($"GET https://localhost:5000/api/books/{id}");

            var book = await DbManager.Context.Books.FindAsync(id);

            if (book == null)
                return NoContent();

            return new OkObjectResult(new BookContentDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Cover = book.Cover,
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
            });
        }

        // POST api/books/save
        [HttpPost("books/save")]
        public async Task<ActionResult<BookPutDTO>> Post([FromBody] BookPutDTO book)
        {
            Console.WriteLine("POST https://localhost:5000/api/books/save");
            Console.WriteLine(JsonConvert.SerializeObject(book, Formatting.Indented));

            if (book == null || book.Title == null || book.Author == null || book.Content == null || book.Genre == null)
                return BadRequest();
            
            var newBook = new BookDetailDTO
            {
                Title = book.Title,
                Author = book.Author,
                Cover = book.Cover,
                Content = book.Content,
                Genre = book.Genre
            };
            int bookId = 0;

            if (book.Id == null)
            {
                await DbManager.Context.Books.AddAsync(newBook);
                bookId = DbManager.Context.Books.ToList().Last().Id + 1; //without SaveChanges element not in db
            }
            else
            {
                var foundBook = await DbManager.Context.Books.FindAsync(book.Id);

                if (foundBook == null)
                {
                    await DbManager.Context.Books.AddAsync(newBook);
                    await DbManager.Context.SaveChangesAsync();

                    if (newBook.Id == 0)
                        return new OkObjectResult(new { id = DbManager.Context.Books.ToList().Last().Id });
                    else
                        return new BadRequestObjectResult(new { id = DbManager.Context.Books.ToList().Last().Id });
                }
                else
                {
                    foundBook.Title = book.Title;
                    foundBook.Author = book.Author;
                    foundBook.Cover = book.Cover;
                    foundBook.Content = book.Content;
                    foundBook.Genre = book.Genre;

                    bookId = foundBook.Id;
                }

            }

            await DbManager.Context.SaveChangesAsync();

            return CreatedAtAction(nameof(Post), new { id = bookId });
        }

        // PUT api/books/{id}/review
        [HttpPut("books/{id}/review")]
        public async Task<ActionResult<ReviewDTO>> Put(int id, ReviewDTO review)
        {
            Console.WriteLine($"PUT https://localhost:5000/api/books/{id}/review");
            Console.WriteLine(JsonConvert.SerializeObject(review, Formatting.Indented));

            if (review == null || review.Message == null || review.Reviewer == null)
                return BadRequest();

            var book = await DbManager.Context.Books.FindAsync(id);

            if (book == null)
                return NotFound();

            book.Reviews.Add(new ReviewDetailsDTO
            {
                Message = review.Message,
                Reviewer = review.Reviewer
            });

            await DbManager.Context.SaveChangesAsync();

            return new OkObjectResult(new { id = book.Reviews.ToList().Last().Id });
        }

        // PUT api/books/{id}/rate
        [HttpPut("books/{id}/rate")]
        public async Task<ActionResult<RatingDTO>> Put(int id, RatingDTO rating)
        {
            Console.WriteLine($"PUT https://localhost:5000/api/books/{id}/rate");
            Console.WriteLine(JsonConvert.SerializeObject(rating, Formatting.Indented));

            if (rating == null || rating.Score == null || rating.Score < 1 || rating.Score > 5)
                return BadRequest();

            var book = await DbManager.Context.Books.FindAsync(id);

            if (book == null)
                return NotFound();

            book.Ratings.Add(new RatingDetailsDTO
            {
                Score = rating.Score
            });

            await DbManager.Context.SaveChangesAsync();

            return new OkObjectResult(new { id = book.Ratings.ToList().Last().Id });
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("books/{id}")]
        public async Task<ActionResult<BookDTO>> Delete(int id, string secretKey)
        {
            Console.Write($"DELETE https://localhost:5000/api/books/{id}");
            if (secretKey != null)
                Console.WriteLine($"?secretkey={secretKey}");
            else
                Console.WriteLine();

            if (secretKey == null || secretKey != DbManager.SecretKey)
                return Forbid();

            if (DbManager.Context.Books == null || DbManager.Context.Books.Count() == 0)
                return NotFound();

            var book = await DbManager.Context.Books.FindAsync(id);

            if (book == null)
                return NotFound();

            DbManager.Context.Books.Remove(book);
            await DbManager.Context.SaveChangesAsync();

            return Ok(book);
        }
    }
}