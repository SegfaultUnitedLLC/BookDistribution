using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using BookDistribution.Models;

namespace BookDistribution.Test.Models
{
    [TestFixture]
    class BookTests
    {
        [Test]
        [Ignore("Not implemented yet")]
        public void ConstructorTest()
        {

        }

        public class TestValue
        {
            public Book ReferenceBook { get; set; }
            public string Id { get; set; }
        }
    }
}
