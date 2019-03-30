using CarRent;
using CarRent.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRentTests
{
	[TestClass]
    public class ClientServiceTests
    {
		[TestMethod]
		public void ValidRentCar_CarSavedToRepository()
		{
			var carStub = new CarStub();
			var carRepoStub = new CarRepositoryStub(carStub);
			var clientRepoStub = new ClientRepositoryStub();
			var service = new ClientService(carRepoStub, clientRepoStub);

			service.RentCar(
				Guid.NewGuid(), 
				Guid.NewGuid(), 
				new DatePeriod(DateTimeOffset.Now, DateTimeOffset.Now));

			Assert.IsTrue(carRepoStub.SaveWasCalled);
		}

		private class ClientRepositoryStub : IClientRepository
		{
			public IClient GetClient(Guid clientId)
			{
				throw new NotImplementedException();
			}

			public void SaveClient(IClient client)
			{
				throw new NotImplementedException();
			}
		}

		private class CarRepositoryStub : ICarRepository
		{
			private readonly ICar car;

			public CarRepositoryStub(ICar car)
			{
				this.car = car;
			}

			public ICar[] Cars => new[] { car };

			public ICar GetCar(Guid carId)
			{
				return car;
			}

			public void SaveCar(ICar car)
			{
				SaveWasCalled = true;
			}

			public bool SaveWasCalled { get; private set; }
		}

		private class CarStub : ICar
		{
			public string Color => throw new NotImplementedException();

			public Guid Id => throw new NotImplementedException();

			public string Name => throw new NotImplementedException();

			public IEnumerable<Rent> Rents => throw new NotImplementedException();

			public bool IsAvailableOnPeriod(DatePeriod period)
			{
				throw new NotImplementedException();
			}

			public Rent Rent(DatePeriod period)
			{
				return new Rent(
					Guid.NewGuid(),
					Guid.NewGuid(), 
					new DatePeriod(DateTimeOffset.Now, DateTimeOffset.Now));
			}
		}
	}
}
