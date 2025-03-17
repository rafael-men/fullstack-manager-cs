using System.ComponentModel.DataAnnotations;

namespace main.Dto
{
    namespace main.Models.Dto
    {
        public class ClienteDto
        {

            public int Id { get; set; }

            [Required]
            [MaxLength(255)]
            public string Nome { get; set; }
        }
    }

}
