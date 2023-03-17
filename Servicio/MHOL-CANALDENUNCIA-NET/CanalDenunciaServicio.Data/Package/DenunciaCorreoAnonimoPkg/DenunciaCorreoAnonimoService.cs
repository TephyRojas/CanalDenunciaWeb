using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaCorreoAnonimoPkg
{
    public class DenunciaCorreoAnonimoService
    {
        public int Insert(string conn, DenunciaCorreoAnonimoInputDto entity)
        {
         
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "Sp_InsertDenunciaCorreoAnonimo";
                var affectedRows = db.Execute(strSql,
                new
                {
                    IdDenuncia = entity.IdDenuncia,
                    Email = entity.Email
                },
                    commandType: CommandType.StoredProcedure
                );

                return affectedRows;
            }
        }

        public DenunciaCorreoAnonimoOutputDto SelectByIdDenuncia(string conn, int idDenuncia)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "Sp_SelectDenunciaCorreoAnonimoByIdDenuncia";

                var result = db.Query<DenunciaCorreoAnonimoOutputDto>(strSql,
                    new
                    {
                        IdDenuncia = idDenuncia
                    },
                    commandType: CommandType.StoredProcedure
                ).SingleOrDefault();

                return result;
            }
        }

        //public List<UsuarioOutputDto> SelectAll(string conn)
        //{
        //    using (IDbConnection db = new SqlConnection(conn))
        //    {
        //        string strSql = "USP_sisSistemaSelByDominio";

        //        var result = db.Query<UsuarioOutputDto>(strSql,
        //           commandType: CommandType.StoredProcedure
        //        ).ToList();

        //        return result;
        //    }
        //}

        //public UsuarioOutputDto SelectById(string conn, decimal usuId)
        //{
        //    using (IDbConnection db = new SqlConnection(conn))
        //    {
        //        string strSql = "USP_tokTokenSeltokId";

        //        var result = db.Query<UsuarioOutputDto>(strSql,
        //            new
        //            {
        //                usuId = usuId
        //            },
        //            commandType: CommandType.StoredProcedure
        //        ).SingleOrDefault();

        //        return result;
        //    }
        //}

        //public UsuarioOutputDto SelectByEmail(string conn, string usuEmail)
        //{
        //    using (IDbConnection db = new SqlConnection(conn))
        //    {
        //        string strSql = "USP_usuUsuarioSelusuEmail";

        //        var result = db.Query<UsuarioOutputDto>(strSql,
        //            new
        //            {
        //                usuEmail = usuEmail
        //            },
        //            commandType: CommandType.StoredProcedure
        //        ).SingleOrDefault();

        //        return result;
        //    }
        //}

        //public UsuarioSistemaRolOutputDto SelectByEmailSistema(string conn, string usuEmail, int idSistema)
        //{
        //    using (IDbConnection db = new SqlConnection(conn))
        //    {
        //        string strSql = "SSO_RolUsuarioSistema";

        //        var result = db.Query<UsuarioSistemaRolOutputDto>(strSql,
        //            new
        //            {
        //                Email = usuEmail,
        //                IDsistema = idSistema
        //            },
        //            commandType: CommandType.StoredProcedure
        //        ).SingleOrDefault();

        //        return result;
        //    }
        //}

        //public UsuarioOutputDto SelectLogin(string conn, UsuarioLoginInputDto entity)
        //{
        //    using (IDbConnection db = new SqlConnection(conn))
        //    {
        //        string strSql = "USP_usuUsuarioSelLogin";

        //        var result = db.Query<UsuarioOutputDto>(strSql,
        //           new
        //           {
        //               usuEmail = entity.usuEmail,
        //               usuPassword = entity.usuPassword,
        //               sisId = entity.sisId
        //           },
        //           commandType: CommandType.StoredProcedure
        //       ).SingleOrDefault();

        //        return result;
        //    }
        //}


        //public int Update(string conn, UsuarioInputDto entity)
        //{
        //    using (IDbConnection db = new SqlConnection(conn))
        //    {
        //        string strSql = "USP_usuUsuarioUpd";
        //        var affectedRows = db.Execute(strSql,
        //            new
        //            {
        //                usuId = entity.usuId,
        //                usuEmail = entity.usuEmail,
        //                usuPassword = entity.usuPassword,
        //                usuNombre = entity.usuNombre,
        //                usuActivo = entity.usuActivo
        //            },
        //            commandType: CommandType.StoredProcedure
        //        );

        //        return affectedRows;
        //    }
        //}

        //public int UpdateActivaUsuario(string conn, decimal usuId)
        //{
        //    using (IDbConnection db = new SqlConnection(conn))
        //    {
        //        string strSql = "USP_usuUsuarioUpdActivoUsuario";
        //        var affectedRows = db.Execute(strSql,
        //            new
        //            {
        //                usuId = usuId
        //            },
        //            commandType: CommandType.StoredProcedure
        //        );

        //        return affectedRows;
        //    }
        //}

        //public int UpdatePassword(string conn, UsuarioPasswordInputDto entity)
        //{
        //    using (IDbConnection db = new SqlConnection(conn))
        //    {
        //        string strSql = "USP_usuUsuarioUpdPassword";
        //        var affectedRows = db.Execute(strSql,
        //            new
        //            {
        //                usuId = entity.usuId,
        //                usuPassword = entity.usuPassword,
        //                usuActivo = entity.usuActivo
        //            },
        //            commandType: CommandType.StoredProcedure
        //        );

        //        return affectedRows;
        //    }
        //}

        //public int Delete(string conn, decimal usuId)
        //{
        //    using (IDbConnection db = new SqlConnection(conn))
        //    {
        //        string strSql = "USP_usuUsuarioDel";
        //        var affectedRows = db.Execute(strSql,
        //            new
        //            {
        //                usuId = usuId
        //            },
        //            commandType: CommandType.StoredProcedure
        //        );

        //        return affectedRows;
        //    }
        //}
    }
}
