using System.ComponentModel.DataAnnotations;

namespace CanalDenunciaServicio.Data.Package.UsuarioPkg
{
    public class UsuarioLoginInputDto
    {
        [Required]
        [StringLength (12)]
        public string Rut { get; set; }
        [Required]
        [StringLength(64)]
        public string Password { get; set; }
    }
}
