using System;

namespace CarRent
{
	public interface IClientRepository
	{
		IClient GetClient(Guid clientId);
		void SaveClient(IClient client);
	}
}