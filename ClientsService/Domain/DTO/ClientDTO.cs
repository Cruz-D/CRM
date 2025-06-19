using System.Text.Json.Serialization;

public class ClientDTO
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("firstLastName")]
    public string FirstLastName { get; set; }
    [JsonPropertyName("secondLastName")]
    public string SecondLastName { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }
    [JsonPropertyName("phone")]
    public int Phone { get; set; }
    [JsonPropertyName("company")]
    public string Company { get; set; }
    [JsonPropertyName("companyPosition")]
    public string CompanyPosition { get; set; }
    [JsonPropertyName("addressCompany")]
    public CompanyAddressDTO AddressCompany { get; set; }
}

public class CompanyAddressDTO
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
