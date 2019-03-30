using CarRent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CarRentTests
{
    [TestClass]
    public class DatePeriodTests
    {
        [TestMethod]
        public void IntersectTwoNotOverlappingPeriods_ReturnFalse()
        {
			var firstPeriodStartDate = DateTimeOffset.Now;
			var firstPeriodEndDate = firstPeriodStartDate.AddDays(5);
			var secondPeriodStartDate = firstPeriodEndDate.AddDays(2);
			var secondPeriodEndDate = secondPeriodStartDate.AddDays(6);
			var firstPeriod = new DatePeriod(firstPeriodStartDate, firstPeriodEndDate);
			var secondPeriod = new DatePeriod(secondPeriodStartDate, secondPeriodEndDate);

			var intersect = firstPeriod.IntersectsWith(secondPeriod);

			Assert.IsFalse(intersect);
		}

		[TestMethod]
		public void IntersectTwoOverlappingPeriods_ReturnTrue()
		{
			var firstPeriodStartDate = DateTimeOffset.Now;
			var firstPeriodEndDate = firstPeriodStartDate.AddDays(5);
			var secondPeriodStartDate = firstPeriodStartDate.AddDays(2);
			var secondPeriodEndDate = secondPeriodStartDate.AddDays(4);
			var firstPeriod = new DatePeriod(firstPeriodStartDate, firstPeriodEndDate);
			var secondPeriod = new DatePeriod(secondPeriodStartDate, secondPeriodEndDate);

			var intersect = firstPeriod.IntersectsWith(secondPeriod);

			Assert.IsTrue(intersect);
		}

		[TestMethod]
		public void DatePeriodEquals_ReturnTrue()
		{
			var start = DateTimeOffset.Now;
			var end = DateTimeOffset.Now.AddDays(1);
			var firstPeriod = new DatePeriod(start, end);
			var secondPeriod = new DatePeriod(start, end);

			var areEqual = firstPeriod == secondPeriod;

			Assert.IsTrue(areEqual);
		}
    }
}
