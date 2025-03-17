namespace main.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Cliente : Pessoa
    {
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } 
    }

}
