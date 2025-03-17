namespace main.Dto
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using global::main.Models.Enums;

    public class ProcessoDto
    {
        [Required]
        [MaxLength(50)]
        public string Numero { get; set; }

        [Required]
        public OrgaoResponsavel Orgao { get; set; }

        [Required]
        [MaxLength(255)]
        public string Assunto { get; set; }

        [Required]
        public StatusProcesso Status { get; set; }

        [Required]
        public List<int> ClientesIds { get; set; } = new List<int>();

        [Required]
        public int ProcuradorId { get; set; }

        [Required]
        public int PrazoId { get; set; }

        [Required]
        public int DocumentoId { get; set; }
    }
}
