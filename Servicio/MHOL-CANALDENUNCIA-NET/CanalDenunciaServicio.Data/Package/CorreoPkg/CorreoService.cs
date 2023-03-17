using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CanalDenunciaServicio.Data.Package.CorreoPkg
{
    public class CorreoService
    {
		public CorreoOutputDto SelectCorreo(string conn, string identificador)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "SELECT TOP (1) [IdCorreo] " +
								 " ,[Identificador] " +
								 " ,[Descripcion] " +
								 " ,[Asunto] " +
								 " ,[Cuerpo] " +
								 " ,[Parametro] " +
							   "  FROM[Correo] " +
							   "  Where[Activo] = 1 " +
							     " and[Identificador] = '" +identificador + "'";


				return (CorreoOutputDto)db.Query<CorreoOutputDto>
					(strSql)
					.SingleOrDefault();
			}
		}
	}
}
