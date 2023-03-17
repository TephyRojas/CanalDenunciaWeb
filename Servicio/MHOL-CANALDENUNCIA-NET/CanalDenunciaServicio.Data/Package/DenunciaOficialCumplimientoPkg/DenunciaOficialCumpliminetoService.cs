using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaOficialCumplimientoPkg
{
    public class DenunciaOficialCumplimientoService
    {
        public int Insert(string conn, DenunciaOficialCumplimientoInputDto entity)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "insert into " +
                    "DenunciaOficialCumplimiento (IdDenuncia,Activo) " +
                    "values(@IdDenuncia,@Activo)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
                var affectedRows = db.QuerySingle<int>(strSql,
                new
                {
                    IdDenuncia = entity.IdDenuncia,
                    Activo = entity.Activo
                }
                );

                return affectedRows;
            }
        }
    }
}
