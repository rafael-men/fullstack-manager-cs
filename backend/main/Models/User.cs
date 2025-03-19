using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BCrypt.Net;

namespace main.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; private set; }

        [Required]
        public string Role { get; set; }

        public string Password { get; set; }


        public void SetPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("A senha não pode ser nula ou vazia.", nameof(password));
            }

            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password); 
        }

       
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash); 
        }
    }
}
