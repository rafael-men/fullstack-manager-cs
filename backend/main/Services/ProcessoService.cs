using System.Linq;
using main.Data;
using main.Dto;
using main.Dto.main.Dto;
using main.Models;
using main.Models.Enums;
using main.Repository;
using Microsoft.EntityFrameworkCore;

namespace main.Services
{
    public class ProcessoService
    {
        private readonly AppDbContext _context;
        private readonly ProcessoRepository _processoRepository;

        public ProcessoService(AppDbContext context,ProcessoRepository processoRepository)
        {
            _context = context;
            _processoRepository = processoRepository;
        }

        public async Task<List<ProcessoDto>> getAll()
        {
            var processos = await _context.Processos
                .Include(p => p.Procurador) 
                .ToListAsync();

            var processoDtos = processos.Select(p => new ProcessoDto
            {
                Numero = p.Numero,
                Orgao = p.Orgao,
                Assunto = p.Assunto,
                Status = p.Status,
                ClientesIds = p.ClientesIds,
                ProcuradorId = p.ProcuradorId,
                PrazoId = p.PrazoId, 
                DocumentoId = p.DocumentoId 
            }).ToList();

            return processoDtos;
        }


        public async Task<Processo> CriarProcesso(ProcessoDto processoDto)
        {
            if (processoDto.ClientesIds == null || !processoDto.ClientesIds.Any())
            {
                throw new ArgumentException("O processo deve ter pelo menos um cliente associado.");
            }

            if (processoDto.ProcuradorId == null)
            {
                throw new ArgumentException("O processo deve possuir um procurador");
            }

            if (processoDto.DocumentoId == null) {
                throw new ArgumentException("O processo deve possuir um Documento");
            }

            if(processoDto.PrazoId == null)
            {
                throw new ArgumentException("O processo deve possuir um prazo");
            }


            if (!Enum.IsDefined(typeof(OrgaoResponsavel), processoDto.Orgao))
            {
                throw new ArgumentException("O órgão responsável deve ter um valor entre 0 e 4.");
            }

            if(!Enum.IsDefined(typeof(StatusProcesso), processoDto.Status))
            {
                throw new ArgumentException("O status deve ter um valor entre 0 e 3");
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

          
            var prazoExiste = await _context.Prazos.AnyAsync(p => p.Id == processoDto.PrazoId);
            if (!prazoExiste)
            {
                throw new KeyNotFoundException($"Prazo com ID {processoDto.PrazoId} não encontrado.");
            }

         
            var documentoExiste = await _context.Documentos.AnyAsync(d => d.Id == processoDto.DocumentoId);
            if (!documentoExiste)
            {
                throw new KeyNotFoundException($"Documento com ID {processoDto.DocumentoId} não encontrado.");
            }

            var processo = new Processo
            {
                Numero = processoDto.Numero,
                Orgao = processoDto.Orgao,
                Assunto = processoDto.Assunto,
                Status = processoDto.Status,
                ProcuradorId = processoDto.ProcuradorId,
                Procurador = procurador,
                ClientesIds = processoDto.ClientesIds,
                PrazoId = processoDto.PrazoId,
                DocumentoId = processoDto.DocumentoId
            };

            _context.Processos.Add(processo);
            await _context.SaveChangesAsync();
            procurador.ProcessosIds.Add(processo.Id);
            _context.Procuradores.Update(procurador);
            await _context.SaveChangesAsync();

            return processo;
        }

        public async Task<Processo> EditarProcesso(int numero, ProcessoDto processoDto)
        {
            var processo = await _context.Processos.FindAsync(numero);
            if (processo == null)
            {
                throw new KeyNotFoundException("Processo não encontrado.");
            }

            if (processoDto.ClientesIds == null || !processoDto.ClientesIds.Any())
            {
                throw new ArgumentException("O processo deve ter pelo menos um cliente associado.");
            }

            if (processoDto.ProcuradorId == null)
            {
                throw new ArgumentException("O processo deve possuir um procurador");
            }

            if (processoDto.DocumentoId == null)
            {
                throw new ArgumentException("O processo deve possuir um Documento");
            }

            if (processoDto.PrazoId == null)
            {
                throw new ArgumentException("O processo deve possuir um prazo");
            }

            if (!Enum.IsDefined(typeof(OrgaoResponsavel), processoDto.Orgao))
            {
                throw new ArgumentException("O órgão responsável deve ter um valor entre 0 e 4.");
            }

            if (!Enum.IsDefined(typeof(StatusProcesso), processoDto.Status))
            {
                throw new ArgumentException("O status deve ter um valor entre 0 e 3");
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

            var prazoExiste = await _context.Prazos.AnyAsync(p => p.Id == processoDto.PrazoId);
            if (!prazoExiste)
            {
                throw new KeyNotFoundException($"Prazo com ID {processoDto.PrazoId} não encontrado.");
            }

            var documentoExiste = await _context.Documentos.AnyAsync(d => d.Id == processoDto.DocumentoId);
            if (!documentoExiste)
            {
                throw new KeyNotFoundException($"Documento com ID {processoDto.DocumentoId} não encontrado.");
            }



            if (processo.ProcuradorId != processoDto.ProcuradorId)
            {
               
                if (processo.ProcuradorId != 0)
                {
                    var procuradorAntigo = await _context.Procuradores
                        .FirstOrDefaultAsync(p => p.Id == processo.ProcuradorId);

                    if (procuradorAntigo != null && procuradorAntigo.ProcessosIds.Contains(processo.Id))
                    {
                        procuradorAntigo.ProcessosIds.Remove(processo.Id); 
                        _context.Procuradores.Update(procuradorAntigo); 
                    }
                }

               
                var procuradorNovo = await _context.Procuradores
                    .FirstOrDefaultAsync(p => p.Id == processoDto.ProcuradorId);

               
                if (procuradorNovo == null)
                {
                    throw new KeyNotFoundException("Procurador não encontrado.");
                }

              
                procuradorNovo.ProcessosIds.Add(processo.Id); 
                _context.Procuradores.Update(procuradorNovo);  
            }




            processo.Numero = processoDto.Numero;
            processo.Orgao = processoDto.Orgao;
            processo.Assunto = processoDto.Assunto;
            processo.Status = processoDto.Status;
            processo.ProcuradorId = processoDto.ProcuradorId;
            processo.Procurador = procurador;
            processo.ClientesIds = processoDto.ClientesIds;
            processo.PrazoId = processoDto.PrazoId;
            processo.DocumentoId = processoDto.DocumentoId;

            _context.Processos.Update(processo);
            await _context.SaveChangesAsync();

            return processo;
        }

        public async Task<List<ProcessoDto>> FiltrarProcessos(ProcessoFiltroDto filtroDto)
        {
            var processos = await _processoRepository.FiltrarProcessos(filtroDto);

            return processos.Select(p => new ProcessoDto
            {
                Numero = p.Numero,
                Orgao = p.Orgao,
                Assunto = p.Assunto,
                Status = p.Status,
                ClientesIds = p.ClientesIds,
                ProcuradorId = p.ProcuradorId,
                PrazoId = p.PrazoId,
                DocumentoId = p.DocumentoId
            }).ToList();
        }

        public async Task<bool> DeletarProcesso(string numero)
        {
          
            var processo = await _context.Processos
                                         .FirstOrDefaultAsync(p => p.Numero == numero);
            if (processo == null)
            {
                return false; 
            }


            var procurador = await _context.Procuradores.FindAsync(processo.ProcuradorId);
            if (procurador != null)
            {
                procurador.ProcessosIds.Remove(processo.Id); 
                _context.Procuradores.Update(procurador);
            }

       
            _context.Processos.Remove(processo);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
