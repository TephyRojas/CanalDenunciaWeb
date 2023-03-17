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
    public class ModificacionPasswordService
    {
		public List<ModificacionPasswordOutputDto> SelectPasswordByIdList(string conn, int Id)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "SELECT * FROM( " +
                           "    SELECT password from usuario where IdUsuario = "+Id+" ) AS Tabla1 " +
                           " UNION ALL " +
                           " SELECT* FROM( " +
                           "    SELECT TOP 4 a.password FROM ModificacionPassword a where a.IdUsuario = "+Id+" ORDER BY IdModificacionPassword ) AS Tabla2; "; 

				return (List<ModificacionPasswordOutputDto>)db.Query<ModificacionPasswordOutputDto>
					(strSql)
					.ToList();
			}
		}

        public int Insert(string conn, ModificacionPasswordInputDto entity)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "insert into " +
                    "ModificacionPassword (IdUsuario,Password,FechaCambio) values (@IdUsuario,@Password,@FechaCambio);" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
                var affectedRows = db.QuerySingle<int>(strSql,
                new
                {
                    IdUsuario = entity.IdUsuario,
                    Password = entity.Password,
                    FechaCambio = entity.FechaCambio
                }
                );

                return affectedRows;
            }
        }
    }
}
