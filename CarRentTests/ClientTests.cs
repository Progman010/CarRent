using CarRent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CarRentTests
{
	[TestClass]
    public class ClientTests
    {
		[TestMethod]
		public void ClientHasRentsOnPeriodAndCallRentAvailable_ReturnFalse()
		{
			var rentStartPeriod = DateTimeOffset.Now;
			var rentEndPeriod = rentStartPeriod.AddDays(15);
			var client = CreateClient(rentStartPeriod, rentEndPeriod);
			var requestedPeriod = new DatePeriod(
				rentStartPeriod.AddDays(4), 
				rentStartPeriod.AddDays(8));

			var canRent = client.CanRentAtPeriod(requestedPeriod);

			Assert.IsFalse(canRent);
		}

		[TestMethod]
		public void ClientHasNoRentsOnPeriodAndCallRentAvailable_ReturnTrue()
		{
			var rentStartPeriod = DateTimeOffset.Now;
			var rentEndPeriod = rentStartPeriod.AddDays(3);
			var client = CreateClient(rentStartPeriod, rentEndPeriod);
			var requestedPeriod = new DatePeriod(
				rentStartPeriod.AddDays(4),
				rentStartPeriod.AddDays(5));

			var canRent = client.CanRentAtPeriod(requestedPeriod);

			Assert.IsTrue(canRent);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void AddRentToClientWithRentOnThisPeriod_ThrowsException()
		{
			var rentStartPeriod = DateTimeOffset.Now;
			var rentEndPeriod = rentStartPeriod.AddDays(7);
			var client = CreateClient(rentStartPeriod, rentEndPeriod);
			var rent = CreateRent(
				rentStartPeriod.AddDays(3),
				rentStartPeriod.AddDays(9));

			client.AddRent(rent);
		}

		[TestMethod]
		public void AddRentToClientWithoutRentOnThisPeriod_AddsNewRent()
		{
			var rentStartPeriod = DateTimeOffset.Now;
			var rentEndPeriod = rentStartPeriod.AddDays(2);
			var client = CreateClient(rentStartPeriod, rentEndPeriod);
			var rent = CreateRent(
				rentStartPeriod.AddDays(5),
				rentStartPeriod.AddDays(10));

			client.AddRent(rent);

			CollectionAssert.Contains(client.Rents, rent);
		}

		private Client CreateClient(DateTimeOffset onlyRentStartPeriod, DateTimeOffset onlyRentEndPeriod)
		{
			var rent = CreateRent(onlyRentStartPeriod, onlyRentEndPeriod);
			return new Client(Guid.NewGuid(), "Whatever", new Rent[] { rent });
		}

		private Rent CreateRent(DateTimeOffset rentStartPeriod, DateTimeOffset rentEndPeriod)
		{
			return new Rent(
				Guid.NewGuid(),
				Guid.NewGuid(),
				new DatePeriod(rentStartPeriod, rentEndPeriod));
		}
    }
}
