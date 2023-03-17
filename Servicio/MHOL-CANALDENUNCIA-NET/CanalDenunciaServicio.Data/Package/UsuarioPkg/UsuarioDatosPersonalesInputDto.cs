using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.UsuarioPkg
{
    public class UsuarioDatosPersonalesInputDto
    {
        public int IdUsuario { get; set; }
        [Required]
        [MaxLength(14)]
        public string Telefono { get; set; }
        [Required]
        [MaxLength(248)]
        public string Email { get; set; }
    }
}
