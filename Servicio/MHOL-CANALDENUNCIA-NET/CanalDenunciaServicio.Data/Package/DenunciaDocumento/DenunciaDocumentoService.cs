using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaDocumento
{
    public class DenunciaDocumentoService
    {
        public List<DenunciaDocumentoOutputDto> SelectAll(string conn)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select IdDenunciaDocumento, IdDenuncia, Nombre, Extension, Ruta, Activo from DenunciaDocumento where Activo = 1";
                return (List<DenunciaDocumentoOutputDto>)db.Query<DenunciaDocumentoOutputDto>
                    (strSql)
                    .ToList();
            }
        }

        public DenunciaDocumentoOutputDto SelectById(string conn, decimal IdDenunciaDocumento)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select IdDenunciaDocumento, IdDenuncia, Nombre, Extension, Ruta, Activo from DenunciaDocumento where IdDenunciaDocumento = @IdDenunciaDocumento where Activo = 1";
                return (DenunciaDocumentoOutputDto)db.Query<DenunciaDocumentoOutputDto>
                    (strSql,
                    new
                    {
                        IdDenunciaDocumento = IdDenunciaDocumento
                    })
                    .SingleOrDefault();
            }
        }

        public List<DenunciaDocumentoOutputDto> SelectByIdDenuncia(string conn, decimal IdDenuncia)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select IdDenunciaDocumento, IdDenuncia, Nombre, Extension, Ruta, Activo from DenunciaDocumento where IdDenuncia = @IdDenuncia and Activo = 1";
                return (List<DenunciaDocumentoOutputDto>)db.Query<DenunciaDocumentoOutputDto>
                    (strSql,
                    new
                    {
                        IdDenuncia = IdDenuncia
                    })
                    .ToList();
            }
        }
        public int Insert(string conn, DenunciaDocumentoInputDto entity)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "insert into " +
                    "DenunciaDocumento (IdDenuncia,Nombre,Extension,Ruta,Activo) " +
                    "values(@IdDenuncia,@Nombre,@Extension,@Ruta,@Activo)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
                var affectedRows = db.QuerySingle<int>(strSql,
                new
                {
                    IdDenuncia = entity.IdDenuncia,
                    Nombre = entity.Nombre,
                    Extension = entity.Extension,
                    Ruta = entity.Ruta,
                    Activo = entity.Activo
                }
                );

                return affectedRows;
            }
        }

        public int Update(string conn, DenunciaDocumentoInputDto entity)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "update DenunciaDocumento set Nombre=@Nombre, Extension=@Extension, Ruta = @Ruta ,Activo=@Activo where IdDenunciaDocumento = @IdDenunciaDocumento";
                var affectedRows = db.Execute(strSql,
                    new
                    {
                        //IdDocumentoDenuncia = entity.IdDenunciaDocumento,
                        IdDenuncia = entity.IdDenuncia,
                        Nombre = entity.Nombre,
                        Extension = entity.Extension,
                        Ruta = entity.Ruta,
                        Activo = entity.Activo
                    }
                );

                return affectedRows;
            }
        }

        public int Delete(string conn, decimal IdDenunciaDocumento)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "Delete DenunciaDocumento where IdDenunciaDocumento = @IdDenunciaDocumento";
                var affectedRows = db.Execute(strSql,
                    new
                    {
                        IdDenunciaDocumento = IdDenunciaDocumento
                    }
                );

                return affectedRows;
            }
        }

        public int DeleteByIdDenuncia(string conn, decimal IdDenuncia)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "Delete DenunciaDocumento where IdDenuncia = @IdDenuncia";
                var affectedRows = db.Execute(strSql,
                    new
                    {
                        IdDenuncia = IdDenuncia
                    }
                );

                return affectedRows;
            }
        }
    }
}
