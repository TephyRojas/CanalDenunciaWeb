using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaComentarioPkg
{
    public class DenunciaComentarioInputDto
    {
        public int IdDenuncia { get; set; }
        public int IdUsuario { get; set; }
        public string Comentario { get; set; }
        public string Fecha { get; set; }
        public int Activo { get; set; }
    }
}
