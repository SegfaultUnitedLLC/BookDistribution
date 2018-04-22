using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookDistribution.Models;
using BookDistribution.Utility;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BookDistribution.Controllers
{
    [Route("book/[controller]")]
    public class BookController : Controller
    {
        private readonly BookContext _context;

        public BookController(BookContext context)
        {
            _context = context;
        }

        public BookController() {}

        [HttpGet("{id}")]
        public string Get(string id)
        {
            var db = this.SelectBookContext();
            var book = db.Book.SingleOrDefault(b => b.Id == id);
            return JsonConvert.SerializeObject(book);
        }

        [HttpPost]
        public string Post([FromBody]string value)
        {
            var db = this.SelectBookContext();
            JObject o = JObject.Parse(value);
            var guid = Guid.NewGuid();
            var id = $"bookid-{guid}";
            Book book = new Book($"{id}", (string)o["Title"], (string)o["Author"], (string)o["Publisher"]);
            db.Book.Add(book);
            db.SaveChanges();
            return id;
        }

        [HttpPut("{id}")]
        public void Put(string id, [FromBody]string value)
        {
            var db = this.SelectBookContext();
            JObject o = JObject.Parse(value);
            var book = db.Book.Single(b => b.Id == id);
            book.Title = (string)o["Title"];
            book.Author = (string)o["Author"];
            book.Publisher = (string)o["Publisher"];
            db.Book.Update(book);
            db.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            var db = this.SelectBookContext();
            var book = db.Book.SingleOrDefault(b => b.Id == id);
            db.Book.Remove(book);
            db.SaveChanges();
        }

        private BookContext SelectBookContext()
        {
            if (this._context != null)
            {
                return this._context;
            }
            else
            {
                return new BookContext(new DbContextOptions<BookContext>());
            }
        }
    }
}
