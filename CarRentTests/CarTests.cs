using CarRent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CarRentTests
{
	[TestClass]
    public class CarTests
    {
		[TestMethod]
		public void CanRentOnPeriodOverlappingExistantRent_ReturnFalse()
		{
			var existantRentStartDate = DateTimeOffset.Now;
			var existantRentEndDate = existantRentStartDate.AddDays(3);
			var requestedPeriodStartDate = existantRentStartDate.AddDays(-1);
			var requestedPeriodEndDate = requestedPeriodStartDate.AddDays(2);
			var requestedPeriod = new DatePeriod(requestedPeriodStartDate, requestedPeriodEndDate);
			var car = CreateCarWithSingleRent(existantRentStartDate, existantRentEndDate);

			var isAvailable = car.IsAvailableOnPeriod(requestedPeriod);

			Assert.IsFalse(isAvailable);
		}

		[TestMethod]
		public void CanRentOnPeriodOverlappingMaintenance_ReturnFalse()
		{
			var existantRentStartDate = DateTimeOffset.Now;
			var existantRentEndDate = existantRentStartDate.AddDays(5);
			var requestedPeriodStartDate = existantRentStartDate.AddDays(-15);
			var requestedPeriodEndDate = requestedPeriodStartDate.AddDays(5);
			var maintenancePeriodStartDate = requestedPeriodStartDate.AddDays(3);
			var maintenancePeriodEndDate = maintenancePeriodStartDate.AddDays(5);
			var requestedPeriod = new DatePeriod(requestedPeriodStartDate, requestedPeriodEndDate);
			var car = CreateCarWithSingleRent(
				existantRentStartDate, 
				existantRentEndDate, 
				new DatePeriod(maintenancePeriodStartDate, maintenancePeriodEndDate));

			var isAvailable = car.IsAvailableOnPeriod(requestedPeriod);

			Assert.IsFalse(isAvailable);
		}

		[TestMethod]
		public void CanRentOnPeriodInNotOccupiedPeriod_ReturnTrue()
		{
			var existantRentStartDate = DateTimeOffset.Now;
			var existantRentEndDate = existantRentStartDate.AddDays(3);
			var requestedPeriodStartDate = existantRentStartDate.AddDays(6);
			var requestedPeriodEndDate = requestedPeriodStartDate;
			var requestedPeriod = new DatePeriod(requestedPeriodStartDate, requestedPeriodEndDate);
			var car = CreateCarWithSingleRent(existantRentStartDate, existantRentEndDate);

			var isAvailable = car.IsAvailableOnPeriod(requestedPeriod);

			Assert.IsTrue(isAvailable);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void RentOnPeriodOverlappingExistantRent_ThrowsException()
		{
			var existantRentStartDate = DateTimeOffset.Now;
			var existantRentEndDate = existantRentStartDate.AddDays(6);
			var requestedPeriodStartDate = existantRentStartDate.AddDays(2);
			var requestedPeriodEndDate = requestedPeriodStartDate.AddDays(3);
			var requestedPeriod = new DatePeriod(requestedPeriodStartDate, requestedPeriodEndDate);
			var car = CreateCarWithSingleRent(existantRentStartDate, existantRentEndDate);

			car.Rent(requestedPeriod);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void RentOnPeriodOverlappingMaintenance_ThrowsException()
		{
			var existantRentStartDate = DateTimeOffset.Now;
			var existantRentEndDate = existantRentStartDate.AddDays(5);
			var requestedPeriodStartDate = existantRentStartDate.AddDays(-6);
			var requestedPeriodEndDate = requestedPeriodStartDate.AddDays(5);
			var maintenancePeriodStartDate = requestedPeriodStartDate.AddDays(-2);
			var maintenancePeriodEndDate = maintenancePeriodStartDate.AddDays(4);
			var requestedPeriod = new DatePeriod(requestedPeriodStartDate, requestedPeriodEndDate);
			var car = CreateCarWithSingleRent(
				existantRentStartDate,
				existantRentEndDate,
				new DatePeriod(maintenancePeriodStartDate, maintenancePeriodEndDate));

			car.Rent(requestedPeriod);
		}

		[TestMethod]
		public void RentOnPeriodInNotOccupiedPeriod_AddRentsToList()
		{
			var existantRentStartDate = DateTimeOffset.Now;
			var existantRentEndDate = existantRentStartDate.AddDays(3);
			var requestedPeriodStartDate = existantRentStartDate.AddDays(-5);
			var requestedPeriodEndDate = requestedPeriodStartDate.AddDays(3);
			var requestedPeriod = new DatePeriod(requestedPeriodStartDate, requestedPeriodEndDate);
			var car = CreateCarWithSingleRent(existantRentStartDate, existantRentEndDate);

			var rent = car.Rent(requestedPeriod);
		}

		[TestMethod]
		public void RentLastTimeBeforeMaintenance_MakesUnavailableOnMaintenancePeriod()
		{
			var existantRentStartDate = DateTimeOffset.Now;
			var existantRentEndDate = existantRentStartDate.AddDays(3);
			var lastRentStartDate = existantRentStartDate.AddDays(4);
			var lastRentEndDate = lastRentStartDate.AddDays(3);
			var lastRentPeriod = new DatePeriod(lastRentStartDate, lastRentEndDate);
			var requestedPeriodStartDate = lastRentEndDate.AddDays(1);
			var requestedPeriodEndDate = requestedPeriodStartDate.AddDays(3);
			var requestedPeriod = new DatePeriod(requestedPeriodStartDate, requestedPeriodEndDate);
			var car = CreateCarWithSingleRent(
				existantRentStartDate, 
				existantRentEndDate,
				rentsBeforeMaintenance: 2);

			car.Rent(lastRentPeriod);
			var isAvailableOnMaintenancePeriod = car.IsAvailableOnPeriod(requestedPeriod);

			Assert.IsFalse(isAvailableOnMaintenancePeriod);
		}

		[TestMethod]
		public void RentAndScheduleOnMaintenance_AvailableAfterMaintenancePeriod()
		{
			var existantRentStartDate = DateTimeOffset.Now;
			var existantRentEndDate = existantRentStartDate.AddDays(2);
			var lastRentStartDate = existantRentStartDate.AddDays(3);
			var lastRentEndDate = lastRentStartDate.AddDays(5);
			var lastRentPeriod = new DatePeriod(lastRentStartDate, lastRentEndDate);
			var requestedPeriodStartDate = lastRentEndDate.AddDays(10);
			var requestedPeriodEndDate = requestedPeriodStartDate.AddDays(3);
			var requestedPeriod = new DatePeriod(requestedPeriodStartDate, requestedPeriodEndDate);
			var car = CreateCarWithSingleRent(
				existantRentStartDate,
				existantRentEndDate,
				rentsBeforeMaintenance: 2);

			car.Rent(lastRentPeriod);
			var isAvailableOnPeriodAfterMaintenance = car.IsAvailableOnPeriod(requestedPeriod);

			Assert.IsTrue(isAvailableOnPeriodAfterMaintenance);
		}

		private Car CreateCarWithSingleRent(
			DateTimeOffset rentStartDate, 
			DateTimeOffset rentEndDate,
			DatePeriod maintenancePeriod = null,
			int rentsBeforeMaintenance = 10)
		{
			var carId = Guid.NewGuid();
			var singleRent = new Rent(
				Guid.NewGuid(), 
				carId, 
				new DatePeriod(rentStartDate, rentEndDate));
			var maintenances = maintenancePeriod == null 
				? Array.Empty<DatePeriod>()
				: new DatePeriod[] { maintenancePeriod };
			return new Car(
				carId,
				"Car", 
				"Color", 
				maintenances,
				new Rent[] { singleRent }, 
				rentsBeforeMaintenance, 
				5);
		}
	}
}
