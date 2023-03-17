using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaInvolucradoPkg
{
    public class DenunciaInvolucradoListOutputDto
    {
        public int idDenunciaInvolucrado { get; set; }
        public int idUsuario { get; set; }
        public int IdDenuncia { get; set; }
        public string Nombre { get; set; }
        public string Sede { get; set; }
        public string Departamento { get; set; }
    }
}
