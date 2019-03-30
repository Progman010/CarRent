using System;

namespace CarRent.Application
{
	public interface IClientService
	{
		void RentCar(Guid clientId, Guid carId, DatePeriod period);
		ICar[] ShowCarsAvailable(Guid clientId, DatePeriod datePeriod);
		Rent[] ShowRents(Guid clientId);
	}
}