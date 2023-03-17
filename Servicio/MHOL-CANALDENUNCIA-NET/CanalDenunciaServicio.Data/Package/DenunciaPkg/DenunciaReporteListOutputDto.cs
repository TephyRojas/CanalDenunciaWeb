using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaPkg
{
    public class DenunciaReporteListOutputDto
    {
        public int IdDenuncia { get; set; }
        public string Denuncia { get; set; }
        public string Denunciante { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public string Sede { get; set; }
        public string Departamento { get; set; }
        public string TipoDelito { get; set; }
        public string Estado { get; set; }
        public int Diferencia { get; set; }
        public string Denunciados { get; set; }
    }
}
