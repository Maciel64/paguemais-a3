using System;
using System.Threading.Tasks;
using Repositories;
using Entities;
using Exceptions;

namespace Services
{
    public class ClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task UpdateAsync(Guid clientId, UpdateClientDTO updateDto)
        {
            var client = await _clientRepository.FindByIdAsync(clientId);
            if (client == null)
            {
                throw new ClientNotFoundException();
            }

            client.Name = updateDto.Name;
            client.Cpf = updateDto.Cpf;
            client.Email = updateDto.Email;
            client.Phone = updateDto.Phone;
            client.BirthDate = updateDto.BirthDate;
            client.UpdatedAt = DateTime.UtcNow;

            await _clientRepository.UpdateAsync(client);
        }

        public async Task<Client> GetClientByIdAsync(Guid clientId)
        {
            var client = await _clientRepository.FindByIdAsync(clientId);
            if (client == null)
            {
                throw new ClientNotFoundException();
            }

            return client;
        }
    }
}
