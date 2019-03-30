using System;
using System.Collections.Generic;

namespace CarRent
{
	public class Client : IClient
	{
		public Client(Guid id, string name, Rent[] rents)
		{
			Id = id;
			Name = name ?? throw new ArgumentNullException(nameof(name));
			_rents = new List<Rent>(rents ?? throw new ArgumentNullException(nameof(rents)));
		}

		public Guid Id { get; }

		public string Name { get; }

		public Rent[] Rents => _rents.ToArray();

		public bool CanRentAtPeriod(DatePeriod period)
		{
			foreach (var rent in _rents)
			{
				if (rent.Period.IntersectsWith(period))
				{
					return false;
				}
			}

			return true;
		}

		public void AddRent(Rent rent)
		{
			foreach (var existantRents in _rents)
			{
				if (existantRents.Id == rent.Id)
				{
					throw new InvalidOperationException(
						$"Rent with id {rent.Id} is already added to user {Id}");
				}
			}

			if (!CanRentAtPeriod(rent.Period))
			{
				throw new InvalidOperationException(
					$"User {Id} can't rent another car for this period");
			}

			_rents.Add(rent);
		}

		private readonly List<Rent> _rents;
    }
}
