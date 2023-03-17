using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.UsuarioPkg
{
    public class UsuarioDatosOutputDto
    {
        public string Nombre { get; set; }
        public string Denuncia { get; set; }
        public string Email { get; set; }
        public string TipoDelito { get; set; }
    }
}
