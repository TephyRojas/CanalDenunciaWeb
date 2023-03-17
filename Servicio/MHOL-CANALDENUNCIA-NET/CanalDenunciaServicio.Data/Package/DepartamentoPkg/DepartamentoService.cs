using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DepartamentoPkg
{
    public class DepartamentoService
    {
		public List<DepartamentoComboboxOutputDto> SelectCombobox(string conn, int idSede)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select a.IdDepartamento, a.Nombre  from Departamento a inner join DepartamentoSede b on a.IdDepartamento = b.IdDepartamento where b.IdSede = "+idSede+" and a.Activo = 1";
				return (List<DepartamentoComboboxOutputDto>)db.Query<DepartamentoComboboxOutputDto>
					(strSql)
					.ToList();
			}
		}
	}
}
