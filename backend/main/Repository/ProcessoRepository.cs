using main.Data;
using main.Dto.main.Dto;
using main.Models;
using main.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace main.Repository
{
    public class ProcessoRepository 
    {
        private readonly AppDbContext _context;

        public ProcessoRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Processo>> FiltrarProcessos(ProcessoFiltroDto filtro)
        {
            var query = _context.Processos.AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Numero))
            {
                query = query.Where(p => p.Numero.Contains(filtro.Numero));
            }

            if (filtro.Orgao.HasValue)
            {
                query = query.Where(p => p.Orgao == (OrgaoResponsavel)filtro.Orgao.Value);
            }

            if (!string.IsNullOrEmpty(filtro.Assunto))
            {
                query = query.Where(p => p.Assunto.Contains(filtro.Assunto));
            }

            if (filtro.Status.HasValue)
            {
                query = query.Where(p => p.Status == (StatusProcesso)filtro.Status.Value);
            }

            if (filtro.ClientesIds != null && filtro.ClientesIds.Any())
            {
                query = query.Where(p => p.ClientesIds.Any(id => filtro.ClientesIds.Contains(id)));
            }

            if (filtro.ProcuradorId.HasValue)
            {
                query = query.Where(p => p.ProcuradorId == filtro.ProcuradorId);
            }

            if (filtro.PrazoId.HasValue)
            {
                query = query.Where(p => p.PrazoId == filtro.PrazoId);
            }

            if (filtro.DocumentoId.HasValue)
            {
                query = query.Where(p => p.DocumentoId == filtro.DocumentoId);
            }

            return await query.ToListAsync();
        }
    }
}
