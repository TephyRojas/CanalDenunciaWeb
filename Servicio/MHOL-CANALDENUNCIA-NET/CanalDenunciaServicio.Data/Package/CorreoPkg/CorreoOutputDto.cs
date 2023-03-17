using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.CorreoPkg
{
    public class CorreoOutputDto
    {
        public int IdCorreo { get; set; }
        public string Identificador { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public string Parametro { get; set; }
    }
}
