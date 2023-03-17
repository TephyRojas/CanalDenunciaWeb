using System;
using System.ComponentModel.DataAnnotations;

namespace CanalDenunciaWeb.Data.Package.UsuarioPkg
{
    public class UsuarioRecuperaContrasenaInputDto
    {
        [Required]
        public Int64 IdUsuario { get; set; }
        [Required(ErrorMessage = "Repetición contraseña es obligatoria")]
       
        public string ContrasenaNuevaRepeticion { get; set; }
        [Required(ErrorMessage = "Contraseña es obligatoria")]
        
        
        [Compare("ContrasenaNuevaRepeticion",ErrorMessage ="Las contraseñas deben ser iguales")]
        public string ContrasenaNueva { get; set; }

    }
}
