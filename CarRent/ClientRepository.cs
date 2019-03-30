using System;
using System.Collections.Generic;

namespace CarRent
{
	public class ClientRepository : IClientRepository
	{
		public IClient GetClient(Guid clientId)
		{
			return TryGetClient(clientId) ?? throw new InvalidOperationException(
				$"Client with id {clientId} not found");
		}

		public void SaveClient(IClient client)
		{
			var existantClient = TryGetClient(client.Id);
			if (existantClient != null)
			{
				_clients.Remove(existantClient);
			}

			_clients.Add(client);
		}

		private IClient TryGetClient(Guid clientId)
		{
			foreach (var client in _clients)
			{
				if (client.Id == clientId)
				{
					return client;
				}
			}

			return null;
		}

		private readonly List<IClient> _clients;
    }
}
