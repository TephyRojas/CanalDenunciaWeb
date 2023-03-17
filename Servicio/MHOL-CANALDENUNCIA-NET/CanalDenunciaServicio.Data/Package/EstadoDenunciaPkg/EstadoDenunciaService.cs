using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.EstadoDenunciaPkg
{
    public class EstadoDenunciaService
    {
		public List<EstadoDenunciaComboboxOutputDto> SelectCombobox(string conn)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select IdEstadoDenuncia, Nombre  from EstadoDenuncia where Activo = 1";
				return (List<EstadoDenunciaComboboxOutputDto>)db.Query<EstadoDenunciaComboboxOutputDto>
					(strSql)
					.ToList();
			}
		}
	}
}
