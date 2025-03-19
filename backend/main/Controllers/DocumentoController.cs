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
    [Route("pge/documentos")]
    public class DocumentoController : ControllerBase
    {
        private readonly DocumentoService _documentoService;

        public DocumentoController(DocumentoService documentoService)
        {
            _documentoService = documentoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Documento>>> GetAll()
        {
            var documentos = await _documentoService.GetAll();
            return Ok(documentos);
        }

     
        [HttpPost("novo")]
        public async Task<ActionResult<Documento>> Create([FromBody] DocumentoDto documentoDto)
        {
            try
            {
                var documento = await _documentoService.Create(documentoDto);
                return CreatedAtAction(nameof(GetAll), new { id = documento.Id }, documento);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("deletar/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _documentoService.Delete(id);
            if (!deleted)
            {
                return NotFound(new { message = "Documento não encontrado." });
            }

            return NoContent();
        }
    }
}
