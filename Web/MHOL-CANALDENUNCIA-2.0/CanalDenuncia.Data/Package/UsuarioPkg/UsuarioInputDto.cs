using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaWeb.Data.Package.UsuarioPkg
{
    public class UsuarioInputDto
    {
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string FechaIngresoContrato { get; set; }
        public string FechaFinContrato { get; set; }
        public int idDepartamentoSede { get; set; }
    }
}
