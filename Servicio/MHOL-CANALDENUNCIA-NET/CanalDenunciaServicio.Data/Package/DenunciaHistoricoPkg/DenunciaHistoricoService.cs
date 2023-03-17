using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaHistoricoPkg
{
    public class DenunciaHistoricoService
    {
        public int Insert(string conn, DenunciaHistoricoInputDto entity)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "insert into " +
                    "DenunciaHistorico (IdDenuncia,IdDepartamentoSede,IdTipoDelito,IdEstadoDenuncia,FechaInicio,FechaFin, IdUsuarioModificacion,FechaModificacion) " +
                    "values(@IdDenuncia, @IdDepartamentoSede,@IdTipoDelito,@IdEstadoDenuncia,@FechaInicio,@FechaFin,@IdUsuarioModificacion,@FechaModificacion)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
                var affectedRows = db.QuerySingle<int>(strSql,
                new
                {
                    IdDenuncia = entity.IdDenuncia,
                    IdDepartamentoSede = entity.IdDepartamentoSede,
                    IdTipoDelito = entity.IdTipoDelito,
                    IdEstadoDenuncia = entity.IdEstadoDenuncia,
                    FechaInicio = entity.FechaInicio,
                    FechaFin = entity.FechaFin,
                    IdUsuarioModificacion = entity.IdUsuarioModificacion,
                    FechaModificacion = entity.FechaModificacion
                }
                );

                return affectedRows;
            }
        }
    }
}
