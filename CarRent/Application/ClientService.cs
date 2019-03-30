using System;
using System.Collections.Generic;

namespace CarRent.Application
{
	public class ClientService : IClientService
	{
		public ClientService(ICarRepository carRepository, IClientRepository clientRepository)
		{
			_carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
			_clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
		}

		public ICar[] ShowCarsAvailable(Guid clientId, DatePeriod datePeriod)
		{
			var client = _clientRepository.GetClient(clientId);

			if (!client.CanRentAtPeriod(datePeriod))
			{
				return Array.Empty<Car>();
			}

			var allCars = _carRepository.Cars;
			var availableCars = new List<ICar>();
			foreach (var car in allCars)
			{
				if (car.IsAvailableOnPeriod(datePeriod))
				{
					availableCars.Add(car);
				}
			}

			return availableCars.ToArray();
		}

		public void RentCar(Guid clientId, Guid carId, DatePeriod period)
		{
			var client = _clientRepository.GetClient(clientId);

			if (!client.CanRentAtPeriod(period))
			{
				throw new InvalidOperationException(
					$"Client with id {clientId} can't rent car at this period");
			}

			var car = _carRepository.GetCar(carId);
			var rent = car.Rent(period);
			client.AddRent(rent);
			_carRepository.SaveCar(car);
			_clientRepository.SaveClient(client);
		}

		public Rent[] ShowRents(Guid clientId)
		{
			var client = _clientRepository.GetClient(clientId);
			if (client == null)
			{
				return Array.Empty<Rent>();
			}

			return client.Rents;
		}

		private readonly ICarRepository _carRepository;
		private readonly IClientRepository _clientRepository;
	}
}
