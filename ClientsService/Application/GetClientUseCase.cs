using ClientsService.Domain;
using ClientsService.Domain.DTO;

namespace ClientsService.Application
{
    public class GetClientUseCase
    {
        private readonly IClientRepository _clientRepository;

        public GetClientUseCase(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client> ExecuteAsync(GetClientDTO getClientDTO)
        {
            try
            {
                // Validate the input ID
                if (getClientDTO == null)
                {
                    throw new ArgumentException("Client ID cannot be null or empty.", nameof(getClientDTO));
                }
                // Get the client by ID from the repository
                var client = await _clientRepository.GetClientByIdAsync(getClientDTO);
                if (client == null)
                {
                    throw new KeyNotFoundException($"Client with ID {getClientDTO} not found.");
                }
                return client;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the client.", ex);
            }
        }
    }
}
