using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using BookDistribution.Utility;

namespace BookDistribution.Test.Utility
{
    [TestFixture]
    class StringUtilityTests
    {
        [Test]
        [TestCase("123123123", "123      123       123")]
        [TestCase("e4fcf8b3-8800-4f0e-b2f9-9ed7771b6887EatPrayLoveBeyonceJeffBezos", "e4fcf8b3-8800-4f0e-b2f9-9ed7771b6887Eat Pray LoveBeyonceJeff Bezos")]
        public void TestWhiteSpaceRemoval(string expected, string input)
        {
            Assert.That(expected, Is.EqualTo(StringUtility.RemoveWhitespace(input)));
        }
    }
}
