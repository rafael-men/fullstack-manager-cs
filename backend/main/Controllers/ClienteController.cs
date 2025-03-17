using main.Dto.main.Models.Dto;
using main.Services;
using Microsoft.AspNetCore.Mvc;

namespace main.Controllers
{
    [Route("pge/clientes")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteDto>>> GetAllClientes()
        {
            try
            {
                var clientes = await _clienteService.GetAllClientes();
                return Ok(clientes);  
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar clientes: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetClienteById(int id)
        {
            
            var cliente = await _clienteService.GetClienteById(id);

            if (cliente == null)
            {
                return NotFound(); 
            }

            return Ok(cliente); 
        }


        [HttpPost("novo")]
        public async Task<ActionResult<ClienteDto>> CreateCliente([FromBody] ClienteDto clienteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCliente = await _clienteService.CreateCliente(clienteDto);

            return CreatedAtAction(nameof(GetClienteById), new { id = createdCliente.Id }, createdCliente);
        }

        [HttpDelete("deletar/{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                var result = await _clienteService.DeleteClienteAsync(id);

                if (!result)
                {
                    return NotFound(); 
                }

                return NoContent(); 
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
