namespace main.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Procurador : Pessoa
    {
        [Required]
        [MaxLength(20)]
        public string OAB { get; set; }

        public List<int> ProcessosIds { get; set; } = new List<int>();

    }

}
