using BookApp.Data;
using BookApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookDBContext dbContext;

        public BookController(BookDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/Book
        [HttpGet]
        public IActionResult GetBooks(int page = 1, int pageSize = 10)
        {
            var books = dbContext.Books
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return Ok(books);
        }

        // GET: api/Book/{id}
        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            var book = dbContext.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        // POST: api/Book
        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("Book cannot be null.");
            }

            dbContext.Books.Add(book);
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // PUT: api/Book/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book updatedBook)
        {
            if (id != updatedBook.Id)
            {
                return BadRequest("ID mismatch.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("All fields are required.");
            }

            var existingBook = dbContext.Books.Find(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.ISBN = updatedBook.ISBN;
            existingBook.PublicationDate = updatedBook.PublicationDate;

            dbContext.Entry(existingBook).State = EntityState.Modified;
            dbContext.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Book/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = dbContext.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            dbContext.Books.Remove(book);
            dbContext.SaveChanges();

            return NoContent();
        }
    }
}
