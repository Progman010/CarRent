using System;

namespace CarRent
{
	public interface ICarRepository
	{
		ICar[] Cars { get; }

		ICar GetCar(Guid carId);
		void SaveCar(ICar car);
	}
}