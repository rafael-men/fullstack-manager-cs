using main.Models;
using Microsoft.EntityFrameworkCore;

namespace main.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Processo> Processos { get; set; }
        public DbSet<Prazo> Prazos { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Procurador> Procuradores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

    }
}