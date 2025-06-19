using System.Text.Json.Serialization;

namespace ClientsService.Domain.DTO
{
    public class GetClientsDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("firstLastName")]
        public string FirstLastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public int Phone { get; set; }

        [JsonPropertyName("company")]
        public string Company { get; set; }

        [JsonPropertyName("companyPosition")]
        public string CompanyPosition { get; set; }

    }
}

