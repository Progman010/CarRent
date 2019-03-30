using System;

namespace CarRent
{
    public class Rent
    {
		public Rent(Guid id, Guid carId, DatePeriod period)
		{
			Id = id;
			CarId = carId;
			Period = period ?? throw new ArgumentNullException(nameof(period));
		}

		public Guid Id { get; }
		public Guid CarId { get; }
		public DatePeriod Period { get; }
    }
}
