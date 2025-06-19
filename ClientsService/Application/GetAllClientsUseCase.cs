using ClientsService.Domain;
using ClientsService.Domain.DTO;

namespace ClientsService.Application
{
    public class GetAllClientsUseCase
    {
        private readonly IClientRepository _clientRepository;

        public GetAllClientsUseCase(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
        }

        public async Task<IEnumerable<GetClientsDTO>> ExecuteAsync()
        {
            try
            {
                // Get all clients from the repository
                var clients = await _clientRepository.GetAllClientsAsync();

                // Map the clients to DTOs
                var clientDTOs = clients.Select(client => new GetClientsDTO
                {
                    Id = client.id,
                    Name = client.Name,
                    FirstLastName = client.FirstLastName,
                    Email = client.Email,
                    Phone = client.Phone,
                    Company = client.Company,
                    CompanyPosition = client.CompanyPosition
                });
                return clientDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all clients.", ex);
            }
        }
    }
}
