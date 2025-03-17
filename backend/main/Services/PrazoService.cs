using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using main.Dto;
using main.Models;
using Microsoft.EntityFrameworkCore;
using main.Models.Enums;
using main.Data;

namespace main.Services
{
    public class PrazoService
    {
        private readonly AppDbContext _context;

        public PrazoService(AppDbContext context)
        {
            _context = context;
        }

      
        public async Task<List<Prazo>> GetAll()
        {
            return await _context.Prazos.ToListAsync();
        }

        public async Task<Prazo> Create(PrazoDto prazoDto)
        {
  
            if (!Enum.IsDefined(typeof(TipoPrazo), prazoDto.Tipo) || (int)prazoDto.Tipo < 0 || (int)prazoDto.Tipo > 2)
            {
                throw new ArgumentException("O tipo de prazo deve ser um valor entre 0 e 2.");
            }

        
            if (!Enum.IsDefined(typeof(StatusPrazo), prazoDto.Status) || (int)prazoDto.Status < 0 || (int)prazoDto.Status > 2)
            {
                throw new ArgumentException("O status do prazo deve ser um valor entre 0 e 2.");
            }

            var prazo = new Prazo
            {
                Tipo = prazoDto.Tipo,
                DataVencimento = prazoDto.DataVencimento,
                Status = prazoDto.Status
            };

            _context.Prazos.Add(prazo);
            await _context.SaveChangesAsync();

            return prazo;
        }


        public async Task<bool> Delete(int id)
        {
            var prazo = await _context.Prazos.FindAsync(id);
            if (prazo == null)
            {
                return false;
            }

            
            bool prazoVinculado = await _context.Processos.AnyAsync(p => p.PrazoId == id);
            if (prazoVinculado)
            {
                throw new InvalidOperationException("Não é possível excluir o prazo, pois ele está associado a um processo.");
            }

            _context.Prazos.Remove(prazo);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
