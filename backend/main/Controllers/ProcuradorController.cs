﻿using main.Dto;
using main.Models;
using main.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main.Controllers
{
    [Authorize(Policy = "Procurador")]
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
               
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
              
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, $"Erro interno ao tentar deletar o procurador: {ex.Message}");
            }
        }

    }
}
