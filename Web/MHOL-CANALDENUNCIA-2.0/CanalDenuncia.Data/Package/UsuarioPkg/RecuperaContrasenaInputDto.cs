using System.ComponentModel.DataAnnotations;

namespace CanalDenunciaWeb.Data.Package.UsuarioPkg
{
    public class RecuperaContrasenaInputDto
    {
        [Required(ErrorMessage = "Rut es obligatorio")]
        public string Rut { get; set; }
    }
}
