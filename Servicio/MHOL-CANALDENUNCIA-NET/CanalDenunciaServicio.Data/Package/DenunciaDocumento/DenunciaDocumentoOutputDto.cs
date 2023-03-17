﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaDocumento
{
    public class DenunciaDocumentoOutputDto
    {
        public int IdDenunciaDocumento { get; set; }
        public int IdDenuncia { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public string Ruta { get; set; }
        public int Activo { get; set; }
    }
}
