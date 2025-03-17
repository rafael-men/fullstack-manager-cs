using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using main.Data;
using main.Dto;
using main.Models;
using main.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace main.Services
{
    public class DocumentoService
    {
        private readonly AppDbContext _context;

        public DocumentoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Documento>> GetAll()
        {
            return await _context.Documentos.ToListAsync();
        }

        public async Task<Documento> Create(DocumentoDto documentoDto)
        {
            if (string.IsNullOrWhiteSpace(documentoDto.Nome))
            {
                throw new ArgumentException("O nome do documento é obrigatório.");
            }

            if (!Enum.IsDefined(typeof(TipoDocumento), documentoDto.Tipo) || (int)documentoDto.Tipo < 0 || (int)documentoDto.Tipo > 5)
            {
                throw new ArgumentException("O tipo do documento deve ser um valor entre 0 e 5.");
            }

            var documento = new Documento
            {
                Nome = documentoDto.Nome,
                Tipo = documentoDto.Tipo,
                DataCadastro = documentoDto.DataCadastro
            };

            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();
            return documento;
        }

        public async Task<bool> Delete(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
            {
                return false;
            }

            
            bool documentoVinculado = await _context.Processos.AnyAsync(p => p.DocumentoId == id);
            if (documentoVinculado)
            {
                throw new InvalidOperationException("Não é possível excluir o documento, pois ele está associado a um processo.");
            }

            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
