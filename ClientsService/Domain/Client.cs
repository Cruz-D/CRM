using ClientsService.Domain.DTO;
using System.Text.Json.Serialization;

namespace ClientsService.Domain
{
    public class Client
    {
        [JsonPropertyName("id")]
        public required string id { get; set; }
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("firstLastName")]
        public required string FirstLastName { get; set; }
        [JsonPropertyName("secondLastName")]
        public required string SecondLastName { get; set; }
        [JsonPropertyName("email")]
        public required string Email { get; set; }
        [JsonPropertyName("phone")]
        public int Phone { get; set; }
        [JsonPropertyName("company")]
        public required string Company { get; set; }
        [JsonPropertyName("companyPosition")]
        public required string CompanyPosition { get; set; }
        [JsonPropertyName("addressCompany")]
        public required CompanyAddress AddressCompany { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        public class CompanyAddress
        {
            [JsonPropertyName("street")]
            public string Street { get; set; }
            [JsonPropertyName("city")]
            public string City { get; set; }
            [JsonPropertyName("state")]
            public string State { get; set; }
            [JsonPropertyName("postalCode")]
            public string PostalCode { get; set; }
            [JsonPropertyName("country")]
            public string Country { get; set; }
        }
    }


    public interface IClientRepository
    {

        Task<Client> GetClientByIdAsync(GetClientDTO clientDTO);
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task AddClientAsync(Client client);
        Task UpdateClientAsync(ClientDTO clientDTO);
        Task DeleteClientAsync(GetClientDTO clientDTO);
    }
}
