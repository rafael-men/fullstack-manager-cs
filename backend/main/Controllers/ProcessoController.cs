using main.Dto;
using main.Models;
using main.Services;
using Microsoft.AspNetCore.Mvc;

namespace main.Controllers
{
    [Route("pge/processos")]
    [ApiController]
    public class ProcessoController : ControllerBase
    {
        private readonly ProcessoService _processoService;

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



    }
}
