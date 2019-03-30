using CarRent.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRent
{
	public class CompositionRoot
	{
		public static CompositionRoot Create(int rentsAmountBeforeMaintenance, int maintenanceLengthInDays)
		{
			var carRepository = new CarRepository();
			var clientRepository = new ClientRepository();
			return new CompositionRoot
			{
				ClientService = new ClientService(
					carRepository,
					clientRepository),
				ManagerService = new ManagerService(
					carRepository,
					clientRepository,
					rentsAmountBeforeMaintenance,
					maintenanceLengthInDays)
			};
		}

		public IClientService ClientService { get; private set; }

		public IManagerService ManagerService { get; private set; }
	}
}
