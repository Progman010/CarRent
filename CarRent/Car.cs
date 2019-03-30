using System;
using System.Collections.Generic;

namespace CarRent
{
	public class Car : ICar
	{
		public Car(
			Guid id, 
			string name,
			string color, 
			DatePeriod[] maintenances, 
			Rent[] rents,
			int rentsAmountBeforeMaintenance,
			int maintenancePeriodInDays)
		{
			Id = id;
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Color = color ?? throw new ArgumentNullException(nameof(color));
			_maintenances = new List<DatePeriod>(
				maintenances ?? throw new ArgumentNullException(nameof(maintenances)));
			_rents = new List<Rent>(rents ?? throw new ArgumentNullException(nameof(rents)));
			_rentsAmountBeforeMaintenance = rentsAmountBeforeMaintenance;
			_maintenancePeriodInDays = maintenancePeriodInDays;
		}

		public Guid Id { get; }

		public string Name { get; }

		public string Color { get; }

		public IEnumerable<Rent> Rents => _rents;
		
		public bool IsAvailableOnPeriod(DatePeriod period)
		{
			foreach (var rent in _rents)
			{
				if (rent.Period.IntersectsWith(period))
				{
					return false;
				}
			}

			foreach (var maintenance in _maintenances)
			{
				if (maintenance.IntersectsWith(period))
				{
					return false;
				}
			}

			return true;
		}

		public Rent Rent(DatePeriod period)
		{
			if (!IsAvailableOnPeriod(period))
			{
				throw new InvalidOperationException(
					$"Car {Id} is unavailable to rent on choisen period");
			}

			var rent = new Rent(Guid.NewGuid(), Id, period);
			_rents.Add(rent);

			ScheduleMaintenanceIfNeeded();

			return rent;
		}

		private void ScheduleMaintenanceIfNeeded()
		{
			var lastMaintenanceEndDate = GetLastMaintenanceEndDate();
			var lastRentEndDate = DateTimeOffset.MinValue;
			var rentsWithoutMaintenance = 0;
			foreach (var existantRent in _rents)
			{
				if (existantRent.Period.Start > lastMaintenanceEndDate)
				{
					rentsWithoutMaintenance++;
				}

				if (existantRent.Period.End > lastRentEndDate)
				{
					lastRentEndDate = existantRent.Period.End;
				}
			}

			if (rentsWithoutMaintenance < _rentsAmountBeforeMaintenance)
			{
				return;
			}

			var maintenanceStartDate = lastRentEndDate.AddDays(1);
			var maintenanceEndDate = maintenanceStartDate.AddDays(_maintenancePeriodInDays);
			_maintenances.Add(new DatePeriod(maintenanceStartDate, maintenanceEndDate));
		}

		private DateTimeOffset GetLastMaintenanceEndDate()
		{
			var lastMaintenanceEndDate = DateTimeOffset.MinValue;
			foreach (var maintenance in _maintenances)
			{
				if (maintenance.End > lastMaintenanceEndDate)
				{
					lastMaintenanceEndDate = maintenance.End;
				}
			}


			return lastMaintenanceEndDate;
		}

		private readonly List<DatePeriod> _maintenances;
		private readonly List<Rent> _rents;
		private readonly int _rentsAmountBeforeMaintenance;
		private readonly int _maintenancePeriodInDays;
	}
}
