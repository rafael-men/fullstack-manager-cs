using System.ComponentModel.DataAnnotations;
using main.Models.Enums;

namespace main.Dto
{
    public class DocumentoDto
    {
        [Required]
        [MaxLength(255)]
        public string Nome { get; set; }

        [Required]
        public TipoDocumento Tipo { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}
