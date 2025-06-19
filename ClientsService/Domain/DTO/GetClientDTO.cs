using System.Text.Json.Serialization;

namespace ClientsService.Domain.DTO
{
    public class GetClientDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

    }
}
