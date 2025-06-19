using ClientsService.Domain;

namespace ClientsService.Application
{
    public class UpdateClientUseCase
    {
        private readonly IClientRepository _clientRepository;

        public UpdateClientUseCase(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
        }

        public async Task ExecuteAsync(ClientDTO updateClientDTO)
        {
            try
            {
                // Validate the input DTO
                if (updateClientDTO == null || string.IsNullOrEmpty(updateClientDTO.Id))
                {
                    throw new ArgumentException("Client ID cannot be null or empty.", nameof(updateClientDTO));
                }

                // Update the client in the repository
                await _clientRepository.UpdateClientAsync(updateClientDTO);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the client.", ex);
            }
        }
    }
}
