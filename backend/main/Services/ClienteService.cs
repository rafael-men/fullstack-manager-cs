using main.Data;
using main.Dto.main.Models.Dto;
using main.Models;
using Microsoft.EntityFrameworkCore;

namespace main.Services
{
    public class ClienteService
    {
        private readonly AppDbContext _context;

        public ClienteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClienteDto>> GetAllClientes()
        {
            var clientes = await _context.Clientes
                .Select(c => new ClienteDto
                {
                    Nome = c.Nome
                })
                .ToListAsync();

            return clientes;
        }

        public async Task<ClienteDto> CreateCliente(ClienteDto clienteDto)
        {
            var cliente = new Cliente
            {
                Nome = clienteDto.Nome
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

       
            return new ClienteDto
            {
                Nome = cliente.Nome
            };
        }

        public async Task<ClienteDto> GetClienteById(int id)
        {
    
            var cliente = await _context.Clientes
                .Where(c => c.Id == id)
                .Select(c => new ClienteDto
                {
                    Nome = c.Nome
                })
                .FirstOrDefaultAsync();

            return cliente;
        }

        public async Task<bool> DeleteClienteAsync(int id)
        {
         
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                throw new BadHttpRequestException("cliente não encontrado");
            }

          
            var clienteAssociado = await _context.Processos
                .AnyAsync(p => p.ClientesIds.Contains(id));

            if (clienteAssociado)
            {
                
                throw new InvalidOperationException("Não é possível deletar o cliente, pois ele está associado a um processo.");
            }

          
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
