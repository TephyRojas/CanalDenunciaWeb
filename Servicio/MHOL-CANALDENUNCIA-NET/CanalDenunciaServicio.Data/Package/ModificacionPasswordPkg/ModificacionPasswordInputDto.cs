using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.ModificacionPasswordPkg
{
    public class ModificacionPasswordInputDto
    {
        public int IdUsuario { get; set; }
        public string FechaCambio { get; set; }
        public string Password { get; set; }
    }
}
