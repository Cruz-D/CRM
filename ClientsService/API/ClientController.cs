using ClientsService.Application;
using ClientsService.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClientsService.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        //import use cases
        private readonly CreateClientUseCase _createClientUseCase;
        private readonly GetAllClientsUseCase _getAllClientsUseCase;
        private readonly GetClientUseCase _getClientUseCase;
        private readonly UpdateClientUseCase _updateClientUseCase;
        private readonly DeleteClientUseCase _deleteClientsUseCase;

        public ClientController(
            CreateClientUseCase createClientUseCase,
            GetAllClientsUseCase getAllClientsUseCase,
            GetClientUseCase getClientUseCase,
            UpdateClientUseCase updateClientUseCase,
            DeleteClientUseCase deleteClientsUseCase)
        {
            _createClientUseCase = createClientUseCase ?? throw new ArgumentNullException(nameof(createClientUseCase));
            _getAllClientsUseCase = getAllClientsUseCase ?? throw new ArgumentNullException(nameof(getAllClientsUseCase));
            _getClientUseCase = getClientUseCase ?? throw new ArgumentException(nameof(getClientUseCase));
            _updateClientUseCase = updateClientUseCase ?? throw new ArgumentNullException(nameof(updateClientUseCase));
            _deleteClientsUseCase = deleteClientsUseCase ?? throw new ArgumentNullException(nameof(deleteClientsUseCase));
        }

        // GET: api/<ClientController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var allClients = await _getAllClientsUseCase.ExecuteAsync();

                return Ok(allClients);
            }
            catch (Exception ex)
            {

                throw new Exception("An error occured in the controller.", ex);
            }
        }

        // GET api/<ClientController>/5
        [HttpGet("{id}/{email}")]
        public async Task<IActionResult> Get(string id, string email)
        {
            try
            {
                var getClientDTO = new GetClientDTO { Id = id, Email = email };
                var getClient = await _getClientUseCase.ExecuteAsync(getClientDTO);

                if (getClient == null)
                {
                    return NotFound($"Client with ID {id} and Email {email} not found.");
                }
                return Ok(getClient);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST api/<ClientController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClientDTO createClientDTO)
        {
            try
            {
                await _createClientUseCase.ExecuteAsync(createClientDTO);

                return Ok(201);

            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred in the controller.", ex);
            }
        }

        // PUT api/<ClientController>/5
        [HttpPut("{id}/{email}")]
        public async Task<IActionResult> Put(string id, [FromBody] ClientDTO updateClientDTO)
        {
            if (updateClientDTO == null)
            {
                throw new ArgumentNullException(nameof(updateClientDTO));
            }
            try
            {
                // Call the use case to update the client
                await _updateClientUseCase.ExecuteAsync(updateClientDTO);

                return Ok(201);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the client.", ex);
            }
        }

        // Updated DELETE api/<ClientController>/5 method to fix CS0815 error  
        [HttpDelete("{id}/{email}")]
        public async Task<IActionResult> Delete(string id, string email)
        {
            try
            {
                var getClientDTO = new GetClientDTO { Id = id, Email = email };
                await _deleteClientsUseCase.ExecuteAsync(getClientDTO); // Removed assignment to a variable since ExecuteAsync returns void  

                return Ok($"Client with ID {id} and Email {email} has been deleted successfully.");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the client.", ex);
            }
        }
    }
}
