using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using main.Models.Enums;

namespace main.Models
{
    public class Processo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Numero { get; set; }

        [Required]
        public OrgaoResponsavel Orgao {  get; set; }

        [Required]
        [MaxLength(255)]
        public string Assunto { get; set; }

        [Required]
        public StatusProcesso Status { get; set; }

        public List<Prazo> Prazos {  get; set; }  = new List<Prazo>();

        public List<Documento> Documentos { get; set; } = new List<Documento>();

        [Required]
        public int ProcuradorId { get; set; }

        [ForeignKey("ProcuradorId")]
        public Procurador Procurador { get; set; }

        [Required]
        public List<int> ClientesIds { get; set; } = new List<int>();

    }
}
