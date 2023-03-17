using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaInvolucradoPkg
{
    public class DenunciaInvolucradoOutputDto
    {
        public int IdDenunciaInvolucrado { get; set; }
        public int idDenuncia { get; set; }
        public int idUsuario { get; set; }
        public int Activo { get; set; }
    }
}
