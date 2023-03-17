using System.ComponentModel.DataAnnotations;

namespace CanalDenunciaWeb.Models
{
    public class LoginDto
    {
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Password { get; set; }
    }
}