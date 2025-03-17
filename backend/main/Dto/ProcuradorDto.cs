using System.ComponentModel.DataAnnotations;

namespace main.Dto
{
    public class ProcuradorDto
    {
        [Required]
        [MaxLength(20)]
        public string OAB { get; set; }

        public List<int> ProcessosIds { get; set; } = new List<int>();
    }
}
