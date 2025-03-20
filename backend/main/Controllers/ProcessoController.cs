using main.Data;
using main.Dto;
using main.Dto.main.Dto;
using main.Models;
using main.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace main.Controllers
{
   
    [Route("pge/processos")]
    [ApiController]
    public class ProcessoController : ControllerBase
    {
        private readonly ProcessoService _processoService;
        private readonly AppDbContext _context;

        public ProcessoController(ProcessoService processoService)
        {
            _processoService = processoService;
        }

        
        [HttpGet]
        public async Task<IActionResult> getAllProcessos()
        {
            var processos = await _processoService.getAll();
            return Ok(processos);
        }

        [Authorize(Policy = "Procurador")]
        [HttpPost("novo")]
        public async Task<ActionResult<Processo>> Create([FromBody] ProcessoDto processoDto)
        {
          
            if (processoDto == null)
            {
                return BadRequest("Os dados do processo não foram fornecidos.");
            }

           
         
                var processoCriado = await _processoService.CriarProcesso(processoDto);


            return CreatedAtAction(nameof(getAllProcessos), new { id = processoCriado.Id }, processoCriado);
         
        }

        [Authorize(Policy = "Procurador")]
        [HttpPut("atualizar/{numero}")]
        public async Task<ActionResult> EditarProcesso(int numero, [FromBody] ProcessoDto processoDto)
        {
            await _processoService.EditarProcesso(numero, processoDto);
            
            return NoContent();
        }

       
        [HttpGet("filtrar")]
        public async Task<ActionResult<List<ProcessoDto>>> FiltrarProcessos([FromQuery] ProcessoFiltroDto filtroDto)
        {
            var processos = await _processoService.FiltrarProcessos(filtroDto);
            return Ok(processos);
        }

        [Authorize(Policy = "Procurador")]
        [HttpDelete("deletar/{numero}")]
        public async Task<ActionResult> DeletarProcesso(string numero)
        {
            var sucesso = await _processoService.DeletarProcesso(numero);
            if (!sucesso)
            {
                return NotFound("Processo não encontrado ou não pode ser deletado.");
            }
            return NoContent();
        }


    }
}
