using BookDistribution.Controllers;
using BookDistribution.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;

namespace BookDistribution.Test.Controllers
{
    [TestFixture]
    class BookControllerTests
    {
        [Test, TestCaseSource(typeof(BookControllerTests), nameof(GetTestCases))]
        public void GetBookTest(GetTestValue testValue)
        {
            var controller = new BookController(this.GetContextWithData());
            var response = controller.Get(testValue.Id);
            Assert.That(response, Is.EqualTo(JsonConvert.SerializeObject(testValue.ReferenceBook)));
        }

        [Test, TestCaseSource(typeof(BookControllerTests), nameof(PostTestCases))]
        public void PostBookTest(PostTestValue testValue)
        {
            var controller = new BookController(this.GetContextWithData());
            var postResponse = controller.Post(testValue.Body.ToString());
            var expectedBook = new Book(postResponse, (string)testValue.Body["Title"], (string)testValue.Body["Author"], (string)testValue.Body["Publisher"]);
            var getResponse = controller.Get(postResponse);
            Assert.That(JsonConvert.SerializeObject(expectedBook), Is.EqualTo(getResponse));
        }

        [Test, TestCaseSource(typeof(BookControllerTests), nameof(PutTestCases))]
        public void PutBookTest(PutTestValue testValue)
        {
            var controller = new BookController(this.GetContextWithData());
            controller.Put(testValue.Id, testValue.Body.ToString());
            var expectedBook = new Book(testValue.Id, (string)testValue.Body["Title"], (string)testValue.Body["Author"], (string)testValue.Body["Publisher"]);
            var getResponse = controller.Get(testValue.Id);
            Assert.That(JsonConvert.SerializeObject(expectedBook), Is.EqualTo(getResponse));
        }

        [TestCase("bookid-33cc9576-1d68-4d0c-8b0c-c0d1f7269c04")]
        public void DeleteBookTest(string id)
        {
            var controller = new BookController(this.GetContextWithData());
            controller.Delete(id);
            Assert.That("null", Is.EqualTo(controller.Get(id)));
        }

        public static IEnumerable GetTestCases
        {
            get
            {
                yield return new GetTestValue()
                {
                    ReferenceBook = new Book("aposdiufaposifuLoveBobBob", "bob", "bob", "bob"),
                    Id = "aposdiufaposifuLoveBobBob"
                };
                yield return new GetTestValue()
                {
                    ReferenceBook = new Book("bookid-33cc9576-1d68-4d0c-8b0c-c0d1f7269c04", "Eat Pray Love", "Beyonce", "Jeff Bezos"),
                    Id = "bookid-33cc9576-1d68-4d0c-8b0c-c0d1f7269c04"
                };
                yield return new GetTestValue()
                {
                    ReferenceBook = null,
                    Id = "cal"
                };
            }
        }

        public static IEnumerable PostTestCases
        {
            get
            {
                yield return new PostTestValue()
                {
                    Body = JObject.FromObject(new { Title = "Eat Pray Love", Author = "Beyonce", Publisher = "Jeff Bezos" })
                };
            }
        }

        public static IEnumerable PutTestCases
        {
            get
            {
                yield return new PutTestValue()
                {
                    Body = JObject.FromObject(new { Title = "Harry Potter and Bob Saget", Author = "JK Rowling", Publisher = "Matt Luck" }),
                    Id = "bookid-33cc9576-1d68-4d0c-8b0c-c0d1f7269c04"
                };
            }
        }

        public class GetTestValue
        {
            public Book ReferenceBook { get; set; }
            public string Id { get; set; }
        }

        public class PostTestValue
        {
            public JObject Body { get; set; }
        }

        public class PutTestValue
        {
            public JObject Body { get; set; }
            public string Id { get; set; }
        }

        private BookContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new BookContext(options);

            context.Book.Add(new Book("aposdiufaposifuLoveBobBob", "bob", "bob", "bob"));
            context.Book.Add(new Book("bookid-33cc9576-1d68-4d0c-8b0c-c0d1f7269c04", "Eat Pray Love", "Beyonce", "Jeff Bezos"));
            context.SaveChanges();

            return context;
        }

        private BookContext GetSqlContext()
        {
            var options = new DbContextOptionsBuilder<BookContext>()
                .UseSqlServer("Server = localhost\\SQLEXPRESS; Database = BookDistribution; Trusted_Connection = True;")
                .Options;

            var context = new BookContext(options);
            return context;
        }
    }
}
