using System;
using System.ComponentModel.DataAnnotations;
using main.Models.Enums;

namespace main.Dto
{
    public class PrazoDto
    {
        [Required]
        public TipoPrazo Tipo { get; set; }

        [Required]
        public DateTime DataVencimento { get; set; }

        [Required]
        public StatusPrazo Status { get; set; }
    }
}
