using ClientsService.Domain;
using ClientsService.Domain.DTO;
using static ClientsService.Domain.Client;

namespace ClientsService.Application
{
    public class CreateClientUseCase
    {
        private readonly IClientRepository _clientRepository;

        public CreateClientUseCase(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
        }

        public async Task ExecuteAsync(ClientDTO createClientDTO)
        {

            try
            {
                // Validate the input DTO
                Validate(createClientDTO);

                //Create a new client
                var newClient = new Client
                {
                    id = Guid.NewGuid().ToString(),
                    Name = createClientDTO.Name,
                    FirstLastName = createClientDTO.FirstLastName,
                    SecondLastName = createClientDTO.SecondLastName,
                    Company = createClientDTO.Company,
                    CompanyPosition = createClientDTO.CompanyPosition,
                    Phone = createClientDTO.Phone,
                    Email = createClientDTO.Email,

                    AddressCompany = new CompanyAddress
                    {
                        Street = createClientDTO.AddressCompany.Street,
                        City = createClientDTO.AddressCompany.City,
                        State = createClientDTO.AddressCompany.State,
                        PostalCode = createClientDTO.AddressCompany.PostalCode,
                        Country = createClientDTO.AddressCompany.Country
                    }
                };

                if (newClient != null)
                {

                    await _clientRepository.AddClientAsync(newClient);
                }



                
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while creating the client.", ex);
            }
            
        }

        private void Validate(ClientDTO createClientDTO)
        {
            if (createClientDTO == null)
            {
                throw new ArgumentNullException(nameof(createClientDTO));
            }

            switch (createClientDTO)
            {
                case { Name: var name } when string.IsNullOrWhiteSpace(name):
                    throw new ArgumentException("Client name cannot be empty.", nameof(createClientDTO.Name));

                case { FirstLastName: var firstLastName } when string.IsNullOrWhiteSpace(firstLastName):
                    throw new ArgumentException("Client first last name cannot be empty.", nameof(createClientDTO.FirstLastName));

                case { SecondLastName: var secondLastName } when string.IsNullOrWhiteSpace(secondLastName):
                    throw new ArgumentException("Client second last name cannot be empty.", nameof(createClientDTO.SecondLastName));

                case { Company: var company } when string.IsNullOrWhiteSpace(company):
                    throw new ArgumentException("Client company cannot be empty.", nameof(createClientDTO.Company));

                case { CompanyPosition: var companyPosition } when string.IsNullOrWhiteSpace(companyPosition):
                    throw new ArgumentException("Client company position cannot be empty.", nameof(createClientDTO.CompanyPosition));

                case { Phone: var phone } when phone <= 0:
                    throw new ArgumentException("Client phone must be a positive integer.", nameof(createClientDTO.Phone));

                case { Email: var email } when string.IsNullOrWhiteSpace(email):
                    throw new ArgumentException("Client email cannot be empty.", nameof(createClientDTO.Email));

                case { AddressCompany: var address } when address == null ||
                    string.IsNullOrWhiteSpace(address.Street) ||
                    string.IsNullOrWhiteSpace(address.City) ||
                    string.IsNullOrWhiteSpace(address.State) ||
                    string.IsNullOrWhiteSpace(address.PostalCode) ||
                    string.IsNullOrWhiteSpace(address.Country):
                    throw new ArgumentException("Company address is invalid.", nameof(createClientDTO.AddressCompany));

                default:

                    break;
            }
        }
    }
}
