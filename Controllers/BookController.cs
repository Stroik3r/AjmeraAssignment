using AjmeraAssignment.Data;
using AjmeraAssignment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AjmeraAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly DataContext _context;
        public BookController(ILogger<BookController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            if(book == null || !ModelState.IsValid)
            {
                _logger.LogError($"Invalid Request");
                return BadRequest(StatusCodes.Status406NotAcceptable);
            }

            try
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to Create the Book record due to {ex.Message}");
                return StatusCode(500, ex);   
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                List<Book> books = new List<Book>();
                var results = await _context.Books.ToListAsync();
                if(results.Count == 0)
                {
                    return BadRequest(StatusCodes.Status204NoContent);
                }

                books = results.OrderByDescending(s => s.CreatedOn).ToList();

                return Ok(books);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to fetch the books due to {ex.Message}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            _ = new Book();
            try
            {
                Book? book = await _context.Books.FindAsync(id);
                if (book == null) return NotFound();
                return Ok(book);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get the book's details with ID{id} due to {ex.Message}");
                return StatusCode(500, ex);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book bookUpdate)
        {
            _ = new Book();
            try
            {
                Book? book = await _context.Books.FindAsync(id);
                if (book == null) return NotFound();

                book.Author = bookUpdate.Author;
                book.BookName = bookUpdate.BookName;

                await _context.SaveChangesAsync();

                return Ok(book);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get the book's details with ID{id} due to {ex.Message}");
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var res = await _context.Books.FindAsync(id);
                if (res == null) return NotFound();

                _context.Remove(res);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to delete the book with id{id} due to {ex.Message}");
                return StatusCode(500, ex);
            }
        }
    }
}
