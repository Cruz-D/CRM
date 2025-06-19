using ClientsService.Domain;
using ClientsService.Domain.DTO;
using Microsoft.Azure.Cosmos;


namespace ClientsService.Infrastructure
{
    public class ClientsRepository : IClientRepository
    {
        //Instanciar Container
        private readonly Container _container;

        //Instance Redis
        private readonly RedisCacheService _redisCacheService;

        //Inicializar Constructor
        public ClientsRepository(CosmosClient cosmosClient, Container container, RedisCacheService cacheService  )
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _redisCacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));

        }

        public async Task AddClientAsync(Client client)
        {
            try
            {
                var createRequest = await _container.CreateItemAsync<Client>(client, new PartitionKey(client.Email));
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while adding the client.", ex);
            }
        }

        public Task DeleteClientAsync(GetClientDTO clientDTO)
        {
            try
            {
                _container.DeleteItemAsync<Client>(clientDTO.Id, new PartitionKey(clientDTO.Email));

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            const string cacheKey = "clients:getall";
            try
            {
                var value = await _redisCacheService.GetValueAsync("clients:getall");

                // Intenta obtener los datos del caché
                var cached = await _redisCacheService.GetValueAsync(cacheKey);

                if (!string.IsNullOrEmpty(cached))
                {
                    var clientsList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Client>>(cached);
                    if (clientsList != null)
                        return clientsList;


                    // Si la deserialización falla, continúa para obtener de la base de datos
                }

                // 2. Si no están en caché, consulta la base de datos
                var queryIterator = _container.GetItemQueryIterator<Client>(
                    new QueryDefinition("SELECT " +
                    "c.id, " +
                    "c.Name, " +
                    "c.FirstLastName, " +
                    "c.SecondLastName, " +
                    "c.Email, " +
                    "c.Phone, " +
                    "c.Company, " +
                    "c.CompanyPosition " +
                    "FROM c"));

                var clients = new List<Client>();

                while (queryIterator.HasMoreResults)
                {
                    var response = await queryIterator.ReadNextAsync();
                    clients.AddRange(response);
                }

                // 3. Serializa y almacena en caché
                var serialized = Newtonsoft.Json.JsonConvert.SerializeObject(clients);
                await _redisCacheService.SetValueAsync(cacheKey, serialized);


                return clients;
                
                
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while retrieving all clients.", ex);
            }
        }

        public async Task<Client> GetClientByIdAsync(GetClientDTO client)
        {
            try
            {
                var getClient = await _container.ReadItemAsync<Client>(client.Id, new PartitionKey(client.Email));

                if (getClient == null)
                {
                    throw new Exception("Client not found.");
                }

                return getClient.Resource;
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while retrieving the client by ID.", ex);
            }
        }

        public async Task UpdateClientAsync(ClientDTO clientDTO)
        {
            if (clientDTO == null)
            {
                throw new ArgumentNullException(nameof(clientDTO));
            }

            try
            {
                // Map ClientDTO to Client before replacing the item  
                var client = new Client
                {
                    id = clientDTO.Id,
                    Name = clientDTO.Name,
                    FirstLastName = clientDTO.FirstLastName,
                    SecondLastName = clientDTO.SecondLastName,
                    Email = clientDTO.Email,
                    Phone = clientDTO.Phone,
                    Company = clientDTO.Company,
                    CompanyPosition = clientDTO.CompanyPosition,
                    AddressCompany = new Client.CompanyAddress
                    {
                        Street = clientDTO.AddressCompany.Street,
                        City = clientDTO.AddressCompany.City,
                        State = clientDTO.AddressCompany.State,
                        PostalCode = clientDTO.AddressCompany.PostalCode,
                        Country = clientDTO.AddressCompany.Country
                    },
                    
                    UpdatedAt = DateTime.UtcNow
                };

                var response = await _container.ReplaceItemAsync(client, client.id, new PartitionKey(client.Email));

                if (response == null)
                {
                    throw new Exception("Client not found.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the client.", ex);
            }
        }

    }
}
