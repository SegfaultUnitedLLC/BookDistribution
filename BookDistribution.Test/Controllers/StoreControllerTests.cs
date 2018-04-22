using BookDistribution.Controllers;
using BookDistribution.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BookDistribution.Test.Controllers
{
    [TestFixture]
    class StoreControllerTests
    {
        [Test, TestCaseSource(typeof(StoreControllerTests), nameof(GetTestCases))]
        public void GetStoreTest(GetTestValue testValue)
        {
            var controller = new StoreController(this.GetContextWithData());
            var retrievedStore = controller.Get(testValue.Id);
            Assert.That(JsonConvert.SerializeObject(testValue.ReferenceStore), Is.EqualTo(retrievedStore));
        }

        [Test, TestCaseSource(typeof(StoreControllerTests), nameof(PostTestCases))]
        public void PostStoreTest(PostTestValue testValue)
        {
            var controller = new StoreController(this.GetContextWithData());
            var storeId = controller.Post(testValue.Body.ToString());
            var expectedStore = new Store(storeId, testValue.Body["Body"].ToObject<List<Book>>());
            var retrievedStore = controller.Get(storeId);
            Assert.That(JsonConvert.SerializeObject(expectedStore), Is.EqualTo(retrievedStore));
        }

        [Test,TestCaseSource(typeof(StoreControllerTests), nameof(PutTestCases))]
        public void PutStoreTest(PutTestValue testValue)
        {
            var controller = new StoreController(this.GetContextWithData());
            controller.Put(testValue.Id, testValue.Body.ToString());
            var expectedStore = new Store(testValue.Id, testValue.Body["Body"].ToObject<List<Book>>());
            var retrievedStore = controller.Get(testValue.Id);
            Assert.That(JsonConvert.SerializeObject(expectedStore), Is.EqualTo(retrievedStore));
        }

        [TestCase("aposdiufaposifuLoveBobBob")]
        public void DeleteStoreTest(string id)
        {
            var controller = new StoreController(this.GetContextWithData());
            controller.Delete(id);
            Assert.That("null", Is.EqualTo(controller.Get(id)));
        }

        public static IEnumerable GetTestCases
        {
            get
            {
                yield return new GetTestValue()
                {
                    ReferenceStore = new Store("aposdiufaposifuLoveBobBob", new List<Book>(new Book[] {new Book("bookid-qwerqwer", "Harry Potter", "JK", "Rowling"), new Book("bookid-asdfasdf", "Adventure Time", "Bob", "Dole") })),
                    Id = "aposdiufaposifuLoveBobBob"
                };
            }
        }

        public static IEnumerable PostTestCases
        {
            get
            {
                yield return new PostTestValue()
                {
                    Body = JObject.FromObject(new { Body = new List<Book>(new Book[] { new Book("bookid-spicybook", "Hot Spices", "JK", "Rowling"), new Book("bookid-coldbook", "Cold Drinks", "Bob", "Dole") }) })
                };
            }
        }

        public static IEnumerable PutTestCases
        {
            get
            {
                yield return new PutTestValue()
                {
                    Body = JObject.FromObject(new { Body = new List<Book>(new Book[] { new Book("bookid-spicybook", "Hot Spices", "JK", "Rowling"), new Book("bookid-coldbook", "Cold Drinks", "Bob", "Dole") }) }),
                    Id = "aposdiufaposifuLoveBobBob"
                };
            }
        }

        public class GetTestValue
        {
            public Store ReferenceStore { get; set; }
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

        private StoreContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new StoreContext(options);

            context.Store.Add(new Store("aposdiufaposifuLoveBobBob", new List<Book>(new Book[] { new Book("bookid-qwerqwer", "Harry Potter", "JK", "Rowling"), new Book("bookid-asdfasdf", "Adventure Time", "Bob", "Dole") })));
            context.SaveChanges();

            return context;
        }
    }
}
