using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaDocumentoHistoricoPkg
{
    public class DenunciaDocumentoHistoricoService
    {
        public int Insert(string conn, DenunciaDocumentoHistoricoInputDto entity)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "insert into " +
                    "DenunciaDocumentoHistorico (IdDenunciaDocumento,IdDenuncia,Nombre,Extension,Ruta,FechaModificacion) " +
                    "values(@IdDenunciaDocumento, @IdDenuncia,@Nombre,@Extension,@Ruta,@FechaModificacion)";
                var affectedRows = db.QuerySingle<int>(strSql,
                new
                {
                    IdDenunciaDocumento = entity.IdDenunciaDocumento,
                    IdDenuncia = entity.IdDenuncia,
                    Nombre = entity.Nombre,
                    Extension = entity.Extension,
                    Ruta = entity.Ruta,
                    FechaModificacion = entity.FechaModificacion
                }
                );

                return affectedRows;
            }
        }
    }
}
