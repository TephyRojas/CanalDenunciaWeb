using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.UsuarioPkg
{
    public class UsuarioContrasenaTemporalInputDto
    {
        public Int64 idUsuario { get; set; }
        public string contrasenaTemporal { get; set; }
        public int temporal { get; set; }
        public string FechaCambio{ get; set; }
    }
}
