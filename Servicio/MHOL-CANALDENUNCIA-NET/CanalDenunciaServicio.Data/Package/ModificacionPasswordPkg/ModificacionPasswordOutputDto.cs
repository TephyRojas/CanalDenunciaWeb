using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.ModificacionPasswordPkg
{
    public class ModificacionPasswordOutputDto
    {
        public string Password { get; set; }
    }
}
