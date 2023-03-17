using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DepartamentoSedePkg
{
    public class DepartamentoSedeService
    {

		public List<DepartamentoSedeComboboxOutputDto> SelectCombobox(string conn, int idSede)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select a.IdDepartamentoSede, b.Nombre from DepartamentoSede a inner join Departamento b on a.IdDepartamento = b.IdDepartamento "+
							" where a.IdSede = "+idSede +" and a.Activo = 1 and b.Activo = 1";
				return (List<DepartamentoSedeComboboxOutputDto>)db.Query<DepartamentoSedeComboboxOutputDto>
					(strSql)
					.ToList();
			}
		}

        public int SelectByDepartamentoSede(string conn, string departamento, string sede)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select case isnull(idDepartamentoSede,'0') when  '' then '0' else IdDepartamentoSede end idDepartamentoSede from DepartamentoSede where IdDepartamento = (Select IdDepartamento from Departamento where Nombre = '"+departamento+"') and IdSede = (Select IdSede from Sede where Nombre = '"+sede+"')";
                return (int)db.Query<int>
                    (strSql)
                    .SingleOrDefault();
            }
        }
    }
}
