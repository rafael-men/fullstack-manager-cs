using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using main.Dto;
using main.Models;
using main.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main.Controllers
{
    [Authorize(Policy = "Procurador")]
    [ApiController]
    [Route("pge/prazos")]
    public class PrazoController : ControllerBase
    {
        private readonly PrazoService _prazoService;

        public PrazoController(PrazoService prazoService)
        {
            _prazoService = prazoService;
        }

      
        [HttpGet]
        public async Task<ActionResult<List<Prazo>>> GetAll()
        {
            var prazos = await _prazoService.GetAll();
            return Ok(prazos);
        }

        
        [HttpPost("novo")]
        public async Task<ActionResult<Prazo>> Create([FromBody] PrazoDto prazoDto)
        {
            try
            {
                var prazo = await _prazoService.Create(prazoDto);
                return CreatedAtAction(nameof(GetAll), new { id = prazo.Id }, prazo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

      
        [HttpDelete("deletar/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _prazoService.Delete(id);
            if (!deleted)
            {
                return NotFound(new { message = "Prazo não encontrado." });
            }

            return NoContent();
        }
    }
}
