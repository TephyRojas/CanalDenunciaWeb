using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.UsuarioPkg
{
    public class UsuarioOutputDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Sede { get; set; }
        public string Departamento { get; set; }
    }
}
