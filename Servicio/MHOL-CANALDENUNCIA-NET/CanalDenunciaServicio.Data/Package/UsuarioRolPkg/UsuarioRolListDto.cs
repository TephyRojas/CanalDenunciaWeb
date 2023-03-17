using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.UsuarioRolPkg
{
    public class UsuarioRolListDto
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public int IdUsuario { get; set; }
    }
}
