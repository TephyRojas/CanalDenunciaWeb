using System.ComponentModel.DataAnnotations;

namespace CanalDenunciaWeb.Data.Package.UsuarioPkg
{
    public class UsuarioDatosPersonalesInputDto
    {
        [Required]
        public int IdUsuario { get; set; }
        [MaxLength(14,ErrorMessage ="Telefono excede el maximo de caracteres")]
        public string Telefono { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
