using System;

namespace CarRent.Application
{
	public interface IManagerService
	{
		Guid AddCar(string name, string color);
		Guid RegisterClient(string name);
	}
}