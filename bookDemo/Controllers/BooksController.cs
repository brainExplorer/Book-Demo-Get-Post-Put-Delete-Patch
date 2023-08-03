using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace bookDemo.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = ApplicationContext.Books.ToList();
            return Ok(books);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            //var book = ApplicationContext.Books.SingleOrDefault(x => x.Id == id);
            var book = ApplicationContext.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();
            if (book == null)
            {
                return NotFound(); // 404
            }
            return Ok(book); // 200
        }

        [HttpPost]
        // Book info comes from request's body : FromBody
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                {
                    return BadRequest(); //400
                }
                ApplicationContext.Books.Add(book);
                return StatusCode(201, book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")] // int id for secure coding
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            // check book?
            var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));
            if (entity is null)
            {
                return NotFound(); //404
            }
            // check id
            if (id != book.Id)
                return BadRequest(); //400
            ApplicationContext.Books.Remove(book);
            book.Id = entity.Id;
            ApplicationContext.Books.Add(book);
            return Ok(book);
        }
        [HttpDelete]
        public IActionResult DeleteAllBooks()
        {
            ApplicationContext.Books.Clear();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            var entity = ApplicationContext
                .Books
                .Find(b => b.Id.Equals(id));
            if (entity is null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    message = $"Book with id:{id} could not found"
                });
            }
            ApplicationContext.Books.Remove(entity);
            return NoContent(); // 204
        }


        // We use the "Replace" operation on this project.
        [HttpPatch("{id:int}")]
        public IActionResult PartialyUpdatedOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            // check entry
            var entry = ApplicationContext.Books.Find(b=>b.Id.Equals(id));
            if (entry is null)
            {
                return NotFound(); //404
            }
            bookPatch.ApplyTo(entry);
            return NoContent(); //204
        }


    }
}
