using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using main.Models.Enums;

namespace main.Models
{
    public class Prazo
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public TipoPrazo Tipo { get; set; } 

        [Required]
        public DateTime DataVencimento { get; set; }

        [Required]
        public StatusPrazo Status { get; set; } 

    }
}
