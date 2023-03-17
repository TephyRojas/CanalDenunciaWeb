using CanalDenunciaServicio.Data.Package.UsuarioRolPkg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.UsuarioPkg
{
    public class UsuarioLoginOutputDto
    {
        public string FechaCambioPassword { get; set; }
        public Int64 IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Rut { get; set; }
        public string  Email { get; set; }
        public string Telefono { get; set; }
        public int PasswordTemporal { get; set; }
        public int IdRol { get; set; }
        public int PasswordBloqueada { get; set; }
        public string FechaBloqueo { get; set; }
        public int CantidadRol { get; set; }
    }
}
