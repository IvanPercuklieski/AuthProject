using System.ComponentModel.DataAnnotations;

namespace IBLab.Models
{
    public class User
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(21), MinLength(3)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
