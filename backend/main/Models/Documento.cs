    using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using main.Models.Enums;

namespace main.Models
{
    public class Documento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nome { get; set; } 

        [Required]
        public TipoDocumento Tipo { get; set; } 

        [Required]
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow; 

    }
}
