using System.ComponentModel.DataAnnotations;

namespace IBLab.Models
{
    public class TempUser
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

        public string Code { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
