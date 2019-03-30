using System;
using System.Collections.Generic;

namespace CarRent
{
	public interface ICar
	{
		string Color { get; }
		Guid Id { get; }
		string Name { get; }
		IEnumerable<Rent> Rents { get; }

		bool IsAvailableOnPeriod(DatePeriod period);
		Rent Rent(DatePeriod period);
	}
}