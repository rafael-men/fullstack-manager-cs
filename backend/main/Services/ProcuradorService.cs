using main.Data;
using main.Dto;
using main.Models;
using Microsoft.EntityFrameworkCore;
namespace main.Services
{
    public class ProcuradorService
    {

        private readonly AppDbContext _context;

        public ProcuradorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Procurador>> getAllProcuradores()
        {
            return await _context.Procuradores.ToListAsync();
        }



        public async Task<bool> DeletarProcurador(int procuradorId)
        {
            try
            {

                var procurador = await _context.Procuradores
                    .FirstOrDefaultAsync(p => p.Id == procuradorId);


                if (procurador == null)
                {
                    throw new KeyNotFoundException("Procurador não encontrado.");
                }


                var processosAssociados = await _context.Processos
                    .AnyAsync(p => p.ProcuradorId == procuradorId);

                if (processosAssociados)
                {
                    throw new InvalidOperationException("Não é possível deletar o procurador porque ele está associado a processos.");
                }


                _context.Procuradores.Remove(procurador);


                await _context.SaveChangesAsync();

                return true;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException($"Erro ao deletar procurador: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Erro ao deletar procurador: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro inesperado ao deletar procurador: {ex.Message}");
            }
        }




    }
}
