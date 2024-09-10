using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Account
{
    public class LoginDto
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;
    }
}
