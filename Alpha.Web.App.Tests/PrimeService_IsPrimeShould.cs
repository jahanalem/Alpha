using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Alpha.Web.App.Tests
{
    [TestFixture]
    public class PrimeService_IsPrimeShould
    {
        private PrimeService _primeService;

        [SetUp]
        public void SetUp()
        {
            _primeService = new PrimeService();
        }

        //[Test]
        //public void IsPrime_InputIs1_ReturnFalse()
        //{
        //    var result = _primeService.IsPrime(1);

        //    Assert.IsFalse(result, "1 should not be prime");
        //}

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void IsPrime_InputIs1_ReturnFalse(int value)
        {
            var result = _primeService.IsPrime(value);

            Assert.IsFalse(result, "1 should not be prime");
        }
    }
}
