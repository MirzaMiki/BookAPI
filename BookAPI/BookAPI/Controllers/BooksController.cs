using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookAPI.Controllers
{
    public class BooksController : ApiController
    {
        BookContext bookcontext = new BookContext();
        // GET: api/Books?sort=
        [HttpGet]
        public IHttpActionResult GetBooks(string sort)
        {
            IQueryable<Book> books;
            switch (sort)
            {
                case "asc":
                    books = bookcontext.Books.OrderBy(a => a.Title);
                    break;

                case "desc":
                    books = bookcontext.Books.OrderByDescending(a => a.Title);
                    break;

                default:
                    books = bookcontext.Books;
                    break;
            }
            return Ok(books);
        }

        [HttpGet]
        [Route("api/Books/Paging/{pageNumber=1}/{pageSize=5}")]
        public IHttpActionResult Paging(int pageNumber, int pageSize)
        {
            var books = bookcontext.Books.OrderBy(a => a.Id);
            return Ok(books.Skip((pageNumber - 1) * pageSize).Take(pageSize));
        }

        // GET: api/Books/5
        [HttpGet]
        public IHttpActionResult GetBook(int id)
        {
            try
            {
                var book = bookcontext.Books.Find(id);
                return Ok(book);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // POST: api/Books
        public IHttpActionResult Post([FromBody]Book book)
        {
            try
            {
                bookcontext.Books.Add(book);
                bookcontext.SaveChanges();
                return StatusCode(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Books/5
        public IHttpActionResult Put(int id, [FromBody]Book book)
        {
            try
            {
                var entity = bookcontext.Books.FirstOrDefault(a => a.Id == id);
                entity.Title = book.Title;
                entity.Author = book.Author;
                entity.Description = book.Description;
                bookcontext.SaveChanges();
                return Ok("Record updated...");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Books/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var book = bookcontext.Books.Find(id);
                bookcontext.Books.Remove(book);
                bookcontext.SaveChanges();
                return Ok("Record is deleted...");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
