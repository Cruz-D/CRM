using ClientsService.Domain;
using ClientsService.Domain.DTO;

namespace ClientsService.Application
{
    public class DeleteClientUseCase
    {
        private readonly IClientRepository _clientRepository;

        public DeleteClientUseCase(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
        }

        public async Task ExecuteAsync(GetClientDTO clientDTO)
        {
            try
            {
                // Validate the input DTO
                if (clientDTO == null || string.IsNullOrEmpty(clientDTO.Id))
                {
                    throw new ArgumentException("Client ID cannot be null or empty.", nameof(clientDTO));
                }
                // Delete the client from the repository
                await _clientRepository.DeleteClientAsync(clientDTO);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the client.", ex);
            }
        }
    }
}
