using System.ComponentModel.DataAnnotations;

namespace CanalDenunciaWeb.Data.Package.UsuarioPkg
{
    public class UsuarioLoginInputDto
    {
        [Required]
        public string Rut { get; set; }
        [Required]        
        public string Password { get; set; }
    }
}
