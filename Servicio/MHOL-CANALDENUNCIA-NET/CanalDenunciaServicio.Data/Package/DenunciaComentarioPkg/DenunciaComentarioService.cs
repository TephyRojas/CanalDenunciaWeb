using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaComentarioPkg
{
    public class DenunciaComentarioService
    {
        public int Insert(string conn, DenunciaComentarioInputDto entity)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "insert into DenunciaComentario (IdDenuncia,Usuario,Comentario,Fecha,Activo) values(@IdDenuncia,@Usuario,@Comentario,@Fecha,@Activo)";
                var affectedRows = db.Execute(strSql,
                new
                {
                    IdDenuncia = entity.IdDenuncia,
                    Usuario = entity.IdUsuario,
                    Comentario  = entity.Comentario,
                    Fecha = entity.Fecha,
                    Activo = entity.Activo
                }
                );

                return affectedRows;
            }
        }

        public int DeleteByIdDenuncia(string conn, string idDenuncia)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "Delete DenunciaComentario where IdDenuncia = @IdDenuncia";
                var affectedRows = db.Execute(strSql,
                new
                {
                    IdDenuncia = idDenuncia
                }
                );

                return affectedRows;
            }
        }

        public DenunciaComentarioOutputDto SelectByIdDenunciaComentario(string conn, int idDenunciaComentario)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select (top 1) Comentario, IdDenuncia" +
                    " from DenunciaComentario" +
                    " where idDenunciaComentario = " + idDenunciaComentario;
                return (DenunciaComentarioOutputDto)db.Query<DenunciaComentarioOutputDto>
                    (strSql)
                    .SingleOrDefault();
            }
        }
    }
}
