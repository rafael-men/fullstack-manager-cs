using main.Dto;
using main.Models;
using main.Services;
using Microsoft.AspNetCore.Mvc;

namespace main.Controllers
{
    [Route("pge/procuradores")]
    [ApiController]
    public class ProcuradorController : ControllerBase
    {
        private readonly ProcuradorService _procuradorService;

        public ProcuradorController(ProcuradorService procuradorService)
        {
            _procuradorService = procuradorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Procurador>>> GetAll()
        {
            try
            {
                var procuradores = await _procuradorService.getAllProcuradores();
                return Ok(procuradores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter procuradores: {ex.Message}");
            }
        }

        [HttpPost("novo")]
        public async Task<ActionResult<Procurador>> Create([FromBody] ProcuradorDto procuradorDto)
        {
            try
            {
                if (procuradorDto == null)
                    return BadRequest("Dados do procurador não informados.");

                var procuradorCriado = await _procuradorService.CriarProcurador(procuradorDto);
                return CreatedAtAction(nameof(GetAll), new { id = procuradorCriado.Id }, procuradorCriado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Erro de validação: {ex.Message}");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Erro ao buscar processos: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar procurador: {ex.Message}");
            }
        }

        [HttpDelete("deletar/{procuradorId}")]
        public async Task<IActionResult> DeletarProcurador(int procuradorId)
        {
            try
            {
                bool resultado = await _procuradorService.DeletarProcurador(procuradorId);

                if (!resultado)
                {
                    return NotFound($"Procurador com ID {procuradorId} não encontrado.");
                }

                return Ok($"Procurador com ID {procuradorId} deletado com sucesso.");
            }
            catch (KeyNotFoundException ex)
            {
                // Tratando erro de procurador não encontrado
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Tratando erro de procurador associado a processos
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Tratando outros tipos de erro
                return StatusCode(500, $"Erro interno ao tentar deletar o procurador: {ex.Message}");
            }
        }
    }
}
