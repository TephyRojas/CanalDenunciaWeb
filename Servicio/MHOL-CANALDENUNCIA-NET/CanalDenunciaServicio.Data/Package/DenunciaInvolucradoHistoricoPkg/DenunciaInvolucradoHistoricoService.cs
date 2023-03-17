using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaInvolucradoHistoricoPkg
{
    public class DenunciaInvolucradoHistoricoService
    {
        public int Insert(string conn, DenunciaInvolucradoHistoricoInputDto entity)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "insert into DenunciaInvolucradoHistorico (IdDenunciaInvolucrado, IdDenuncia,IdUsuario,FechaModificacion) values(@IdDenunciaInvolucrado, @IdDenuncia,@IdUsuario,@FechaModificacion)";
                var affectedRows = db.Execute(strSql,
                new
                {
                    IdDenunciaInvolucrado = entity.IdDenunciaInvolucrado,
                    IdDenuncia = entity.idDenuncia,
                    IdUsuario = entity.idUsuario,
                    FechaModificacion = entity.FechaModificacion
                }
                );

                return affectedRows;
            }
        }
    }
}
