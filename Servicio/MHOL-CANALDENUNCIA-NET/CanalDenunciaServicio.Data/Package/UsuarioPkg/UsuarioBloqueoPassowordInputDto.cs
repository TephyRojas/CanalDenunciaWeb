using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.UsuarioPkg
{
    public class UsuarioBloqueoPassowordInputDto
    {
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public string FechaBloqueo { get; set; }
    }
}
