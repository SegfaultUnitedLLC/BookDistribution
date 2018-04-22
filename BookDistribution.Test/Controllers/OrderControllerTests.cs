using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using BookDistribution.Controllers;
using System.Collections.Generic;
using BookDistribution.Models;
using Microsoft.EntityFrameworkCore;

namespace BookDistribution.Test.Controllers
{
    [TestFixture]
    class OrderControllerTests
    {
        [Test, TestCaseSource(typeof(OrderControllerTests), nameof(GetTestCases))]
        public void GetOrderTest(GetTestValue testValue)
        {
            var controller = new OrderController(this.GetContextWithData());
            var retrievedOrder = controller.Get(testValue.Id);
            Assert.That(JsonConvert.SerializeObject(testValue.ReferenceOrder), Is.EqualTo(retrievedOrder));
        }

        [Test, TestCaseSource(typeof(OrderControllerTests), nameof(PostTestCases))]
        public void PostOrderTest(PostTestValue testValue)
        {
            var controller = new OrderController(this.GetContextWithData());
            var orderId = controller.Post(testValue.Body.ToString());
            var expectedOrder = new Order(orderId, testValue.Body["Body"].ToObject<List<Book>>(), (string)testValue.Body["Destination"]);
            var retrievedOrder = controller.Get(orderId);
            Assert.That(JsonConvert.SerializeObject(expectedOrder), Is.EqualTo(retrievedOrder));
        }

        [Test, TestCaseSource(typeof(OrderControllerTests), nameof(PutTestCases))]
        public void PutOrderTest(PutTestValue testValue)
        {
            var controller = new OrderController(this.GetContextWithData());
            controller.Put(testValue.Id, testValue.Body.ToString());
            var expectedOrder = new Order(testValue.Id, testValue.Body["Body"].ToObject<List<Book>>(), (string)testValue.Body["Destination"]);
            var retrievedOrder = controller.Get(testValue.Id);
            Assert.That(JsonConvert.SerializeObject(expectedOrder), Is.EqualTo(retrievedOrder));
        }

        [TestCase("orderid-lalalala")]
        public void DeleteOrderTest(string id)
        {
            var controller = new OrderController(this.GetContextWithData());
            controller.Delete(id);
            Assert.That("null", Is.EqualTo(controller.Get(id)));
        }

        public static IEnumerable GetTestCases
        {
            get
            {
                yield return new GetTestValue()
                {
                    ReferenceOrder = new Order("orderid-lalalala", new List<Book>(new Book[] { new Book("bookid-qwerqwer", "Harry Potter", "JK", "Rowling"), new Book("bookid-asdfasdf", "Adventure Time", "Bob", "Dole") }), "storeid-bob"),
                    Id = "orderid-lalalala"
                };
            }
        }

        public static IEnumerable PostTestCases
        {
            get
            {
                yield return new PostTestValue()
                {
                    Body = JObject.FromObject(new { Body = new List<Book>(new Book[] { new Book("bookid-puripuri", "One Punch Man", "JK", "Rowling"), new Book("bookid-sassy", "Star Trek", "Bob", "Dole") }), Destination = "storeid-shanghai" })
                };
            }
        }

        public static IEnumerable PutTestCases
        {
            get
            {
                yield return new PutTestValue()
                {
                    Body = JObject.FromObject(new { Body = new List<Book>(new Book[] { new Book("bookid-puripuri", "One Punch Man", "JK", "Rowling"), new Book("bookid-sassy", "Star Trek", "Bob", "Dole") }), Destination = "storeid-shanghai" }),
                    Id = "orderid-lalalala"
                };
            }
        }

        public class GetTestValue
        {
            public Order ReferenceOrder { get; set; }
            public string Id { get; set; }
        }

        public class PostTestValue
        {
            public JObject Body { get; set; }
            public string Destination { get; set; }
        }

        public class PutTestValue
        {
            public JObject Body { get; set; }
            public string Id { get; set; }
        }

        private OrderContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<OrderContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new OrderContext(options);

            context.Order.Add(new Order("orderid-lalalala", new List<Book>(new Book[] { new Book("bookid-qwerqwer", "Harry Potter", "JK", "Rowling"), new Book("bookid-asdfasdf", "Adventure Time", "Bob", "Dole") }), "storeid-bob"));
            context.SaveChanges();

            return context;
        }
    }
}
