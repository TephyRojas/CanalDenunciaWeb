using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaPkg
{
    public class DenunciaListOutputDto
    {
        public int IdDenuncia { get; set; }
        public string Denuncia { get; set; }
        public string FechaIngreso { get; set; }
        public string TipoDelito { get; set; }
        public string Estado { get; set; }
    }
}
