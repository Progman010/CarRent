using System;
using System.Collections.Generic;

namespace CarRent
{
	public class CarRepository : ICarRepository
	{
		public ICar[] Cars => _cars.ToArray();

		public ICar GetCar(Guid carId)
		{
			return TryGetCar(carId) ?? throw new InvalidOperationException(
				$"Car with id {carId} not found");
		}

		public void SaveCar(ICar car)
		{
			ICar existantCar = TryGetCar(car.Id);

			if (existantCar != null)
			{
				_cars.Remove(existantCar);
			}

			_cars.Add(car);
		}

		private ICar TryGetCar(Guid carId)
		{
			foreach (var car in _cars)
			{
				if (car.Id == carId)
				{
					return car;
				}
			}

			return null;
		}

		private List<ICar> _cars;
    }
}
