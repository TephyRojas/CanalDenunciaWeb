using System;

namespace CanalDenunciaWeb.Data.Package.UsuarioPkg
{
    public class UsuarioContrasenaTemporalInputDto
    {
        public Int64 idUsuario { get; set; }
        public string ContrasenaTemporal { get; set; }
        public int temporal { get; set; }
        public string FechaCambio { get; set; }
    }
}
