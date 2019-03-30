using System;

namespace CarRent
{
	public interface IClient
	{
		Guid Id { get; }
		string Name { get; }
		Rent[] Rents { get; }

		void AddRent(Rent rent);
		bool CanRentAtPeriod(DatePeriod period);
	}
}