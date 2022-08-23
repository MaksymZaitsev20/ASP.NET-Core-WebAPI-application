using Microsoft.AspNetCore.Mvc;
using Task2.Services;
using Task2.Models;

namespace Task2.Controllers
{
    [Route("api")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        BookService _service;
        
        public BooksController(BookService service)
            => _service = service;

        [HttpGet("recommended")]
        public async Task<ActionResult<BookDTO>> GetTopBooks(string? genre)
        {
            var result = await _service.GetTopBooksAsync(genre);

            return result == null ? NoContent() : new OkObjectResult(result);
        }

        // GET: api/books?order=author
        [HttpGet("books")]
        public async Task<ActionResult<BookDTO>> GetAll(string? order = null)
        {
            var result = await _service.GetAllBooksAsync(order);

            return result == Array.Empty<BookDTO>() ? NoContent() :
                result == null ? BadRequest() :
                new OkObjectResult(result);
        }

        // GET api/<BooksController>/5
        [HttpGet("books/{id}")]
        public async Task<ActionResult<BookContentDTO>> GetById(int id)
        {
            var result = await _service.GetBookByIdAsync(id);

            return result == null ? NoContent() : new OkObjectResult(result);
        }

        // POST api/books/save
        [HttpPost("books/save")]
        public async Task<ActionResult<BookPutDTO>> Post([FromBody] BookPutDTO book)
        {
            var result = await _service.PostAsync(book);

            return result == null ? BadRequest() :
                new OkObjectResult(new { result });
        }

        // PUT api/books/{id}/review
        [HttpPut("books/{id}/review")]
        public async Task<ActionResult<ReviewDTO>> PutReview(int id, ReviewDTO review)
        {
            var result = await _service.PutReviewAsync(id, review);

            return result == null ? NotFound() : new OkObjectResult(result);
        }

        // PUT api/books/{id}/rating
        [HttpPut("books/{id}/rating")]
        public async Task<ActionResult<RatingDTO>> PutRating(int id, RatingDTO rating)
        {
            var result = await _service.PutRatingAsync(id, rating);

            return result == null ? NoContent() : new OkObjectResult(result);
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("books/{id}")]
        public async Task<ActionResult<BookDTO>> Delete(int id, string secretKey)
        {
            var result = await _service.DeleteAsync(id, secretKey);

            return result == null ? Forbid() : new OkObjectResult(result);
        }
    }
}