using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.SedePkg
{
    public class SedeService
    {
		public List<SedeComboboxOutputDto> SelectCombobox(string conn)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select IdSede,Descripcion from Sede where Activo=1";
				return (List<SedeComboboxOutputDto>)db.Query<SedeComboboxOutputDto>
					(strSql)
					.ToList();
			}
		}

		public SedeOutputDto SelectByIdDepartamentoSede(string conn, int id)
        {
			using(IDbConnection db=new SqlConnection(conn))
            {
				string strSql = "select IdSede from DepartamentoSede where Activo = 1 and IdSede ="+id;
				return (SedeOutputDto)db.Query<SedeOutputDto>
					(strSql)
					.SingleOrDefault();
			}
        }
	}
}
