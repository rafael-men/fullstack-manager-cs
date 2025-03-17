using main.Data;
using main.Dto;
using main.Models;
using main.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace main.Services
{
    public class ProcessoService
    {

        private readonly AppDbContext _context;

        public ProcessoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProcessoDto>> getAll()
        {
            var processos = await _context.Processos
                .Include(p => p.Prazos)
                .Include(p => p.Documentos)
                .Include(p => p.Procurador)
                .ToListAsync();

         
            var processoDtos = processos.Select(p => new ProcessoDto
            {
                Numero = p.Numero,
                Orgao = p.Orgao,
                Assunto = p.Assunto,
                Status = p.Status,
                ClientesIds = p.ClientesIds, 
                ProcuradorId = p.ProcuradorId
            }).ToList();

            return processoDtos;
        }


        public async Task<Processo> CriarProcesso(ProcessoDto processoDto)
        {
            if (processoDto.ClientesIds == null || !processoDto.ClientesIds.Any())
            {
                throw new ArgumentException("O processo deve ter pelo menos um cliente associado.");
            }

            if (!Enum.IsDefined(typeof(OrgaoResponsavel), processoDto.Orgao))
            {
                throw new ArgumentException("O órgão responsável deve ter um valor entre 0 e 4.");
            }

            var procurador = await _context.Procuradores.FindAsync(processoDto.ProcuradorId);
            if (procurador == null)
            {
                throw new KeyNotFoundException("Procurador não encontrado.");
            }

            var clientes = await _context.Clientes
                .Where(c => processoDto.ClientesIds.Contains(c.Id))
                .ToListAsync();

            if (clientes.Count != processoDto.ClientesIds.Count)
            {
                var clientesNaoEncontrados = processoDto.ClientesIds
                    .Except(clientes.Select(c => c.Id))
                    .ToList();

                throw new KeyNotFoundException($"Clientes não encontrados: {string.Join(", ", clientesNaoEncontrados)}");
            }

       
            var processo = new Processo
            {
                Numero = processoDto.Numero,
                Orgao = processoDto.Orgao,
                Assunto = processoDto.Assunto,
                Status = processoDto.Status,
                ProcuradorId = processoDto.ProcuradorId,
                Procurador = procurador,
                ClientesIds = processoDto.ClientesIds
            };

 
            _context.Processos.Add(processo);
            await _context.SaveChangesAsync();  

       
            procurador.ProcessosIds.Add(processo.Id);

         
            _context.Procuradores.Update(procurador);
            await _context.SaveChangesAsync();

            return processo;
        }



    }
}
