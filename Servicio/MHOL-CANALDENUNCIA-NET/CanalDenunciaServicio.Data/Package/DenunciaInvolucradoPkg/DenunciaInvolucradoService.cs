using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaInvolucradoPkg
{
    public class DenunciaInvolucradoService
    {
        public List<DenunciaInvolucradoOutputDto> SelectAll(string conn)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select IdDenunciaInvolucrado, IdDenuncia, IdUsuario, Activo from DenunciaInvolucrado";
                return (List<DenunciaInvolucradoOutputDto>)db.Query<DenunciaInvolucradoOutputDto>
                    (strSql)
                    .ToList();
            }
        }

      
        public DenunciaInvolucradoOutputDto SelectById(string conn, decimal IdDenunciaInvolucrado)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select IdDenunciaInvolucrado, IdDenuncia, IdUsuario, Activo from DenunciaInvolucrado where IdDenunciaInvolucrado = @IdDenunciaInvolucrado";
                return (DenunciaInvolucradoOutputDto)db.Query<DenunciaInvolucradoOutputDto>
                    (strSql,
                    new
                    {
                        IdDenunciaInvolucrado = IdDenunciaInvolucrado
                    })
                    .SingleOrDefault();
            }
        }

        public int SelectByIdUsuario(string conn, decimal idDenuncia, decimal idUsuario)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select COUNT(a.idDenuncia) from Denuncia a inner join DenunciaInvolucrado b on a.IdDenuncia = b.IdDenuncia where a.IdDenuncia = @idDenuncia and b.IdUsuario = @idUsuario";
                return (int)db.Query<int>
                    (strSql,
                    new
                    {
                        idDenuncia = idDenuncia,
                        idUsuario = idUsuario
                    })
                    .SingleOrDefault();
            }
        }

        public int SelectByIdRol(string conn, decimal idDenuncia)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select COUNT(a.idUsuario) from UsuarioRol a inner join DenunciaInvolucrado c on a.IdUsuario = c.IdUsuario where a.IdRol = 1 and c.IdDenuncia = "+idDenuncia;
                return (int)db.Query<int>
                    (strSql,
                    new
                    {
                        idDenuncia = idDenuncia
                    })
                    .SingleOrDefault();
            }
        }

        public List<DenunciaInvolucradoListOutputDto> SelectByIdRol(string conn, string rol, string Nombre, int idSede, int idDepartamento, string rolExluido)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select a.IdUsuario," +
                                " a.Nombre + ' ' +a.PrimerApellido Nombre, " +
                                " e.Nombre Sede, " +
                                " f.Nombre Departamento " +
                                " from Usuario a " +
                                " inner join UsuarioRol b on a.IdUsuario = b.IdUsuario " +
                                " inner join Rol c on b.IdRol = c.IdRol " +
                                " inner join DepartamentoSede d on a.IdDepartamentoSede = d.IdDepartamentoSede " +
                                " inner join Sede e on d.IdSede = e.IdSede " +
                                " inner join Departamento f on d.IdDepartamento = f.IdDepartamento " +
                                " where c.IdRol in (" + rol + ") and a.IdUsuario != 1  and a.activo = 1 and b.activo = 1 ";
                                if (rolExluido != "")
                {
                   strSql = strSql + " and a.IdUsuario not in (select z.idusuario from UsuarioRol z where z.IdRol in(1,2,4))";
                }
                               

                if (Nombre != null)
                {
                    if (Nombre.Length > 0)
                    {
                        strSql += " and a.Nombre + ' ' + a.PrimerApellido like '%" + Nombre + "%' ";
                    }
                }
                if (idSede > 0)
                {
                    strSql += " and e.IdSede = " + idSede;
                }

                if (idDepartamento > 0)
                {
                    strSql += " and f.IdDepartamento = " + idDepartamento;
                }


                return (List<DenunciaInvolucradoListOutputDto>)db.Query<DenunciaInvolucradoListOutputDto>
                    (strSql)
                    .ToList();
            }
        }
        public List<DenunciaInvolucradoListOutputDto> SelectListSeleccionado(string conn, int idDenuncia, string rol, string Nombre, int idSede, int idDepartamento)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select a.IdDenuncia " +
                                ", c.Nombre + ' ' + c.PrimerApellido Nombre " +
                                " ,e.Nombre Sede " +
                                " , f.Nombre Departamento " +
                                " from DenunciaInvolucrado a " +
                                " inner join denuncia b on a.IdDenuncia = b.IdDenuncia " +
                                " inner join Usuario c on a.idUsuario = c.IdUsuario " +
                                " inner join DepartamentoSede d on c.IdDepartamentoSede = d.IdDepartamentoSede " +
                                " inner join Sede e on d.IdSede = e.IdSede " +
                                " inner join Departamento f on d.IdDepartamento = f.IdDepartamento " +
                                " inner join UsuarioRol g on c.IdUsuario = g.IdUsuario " +
                                " where g.IdRol in(" + rol + ")";

                if (idDenuncia > 0)
                {
                    strSql += " and a.IdDenuncia = " + idDenuncia;                   
                }
                if(Nombre!=null)
                {
                    if(Nombre.Length>0)
                    {
                        strSql += " and c.Nombre + ' ' + c.PrimerApellido like '%" + Nombre + "%' ";
                    }
                }
                if(idSede> 0)
                {
                    strSql += " and e.IdSede = " + idSede;
                }

                if(idDepartamento > 0)
                {
                    strSql += " and f.IdDepartamento = " + idDepartamento;
                }
                 

                return (List<DenunciaInvolucradoListOutputDto>)db.Query<DenunciaInvolucradoListOutputDto>
                    (strSql)
                    .ToList();
            }
        }
        public List<DenunciaInvolucradoListOutputDto> SelectListNoSeleccionado(string conn, int idDenuncia, string rol, string Nombre, int idSede, int idDepartamento)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select a.IdUsuario," +
                                " a.Nombre + ' ' +a.PrimerApellido Nombre, " +
                                " e.Nombre Sede, " +
                                " f.Nombre Departamento " +
                                " from Usuario a " +
                                " inner join UsuarioRol b on a.IdUsuario = b.IdUsuario " +
                                " inner join Rol c on b.IdRol = c.IdRol " +
                                " inner join DepartamentoSede d on a.IdDepartamentoSede = d.IdDepartamentoSede " +
                                " inner join Sede e on d.IdSede = e.IdSede " +
                                " inner join Departamento f on d.IdDepartamento = f.IdDepartamento " +
                                " where c.IdRol in (" + rol + ") " +
                                " and a.IdUsuario not in(select z.IdUsuario from DenunciaInvolucrado z where z.IdDenuncia= " + idDenuncia + ")";

                if (Nombre != null)
                {
                    if (Nombre.Length > 0)
                    {
                        strSql += " and a.Nombre + ' ' + a.PrimerApellido like '%" + Nombre + "%' ";
                    }
                }
                if (idSede > 0)
                {
                    strSql += " and e.IdSede = " + idSede;
                }

                if (idDepartamento > 0)
                {
                    strSql += " and f.IdDepartamento = " + idDepartamento;
                }


                return (List<DenunciaInvolucradoListOutputDto>)db.Query<DenunciaInvolucradoListOutputDto>
                    (strSql)
                    .ToList();
            }
        }
        public List<DenunciaInvolucradoOutputDto> SelectByIdDenuncia(string conn, int idDenuncia)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select IdDenunciaInvolucrado ," +
                                " IdDenuncia, " +
                                " IdUsuario " +
                                " from DenunciaInvolucrado " +
                                " where IdDenuncia =" + idDenuncia + " ";
                return (List<DenunciaInvolucradoOutputDto>)db.Query<DenunciaInvolucradoOutputDto>
                    (strSql)
                    .ToList();
            }
        }
        public int Insert(string conn, DenunciaInvolucradoInputDto entity)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "insert into DenunciaInvolucrado (IdDenuncia,IdUsuario,Activo) values(@IdDenuncia,@IdUsuario,@Activo)";
                var affectedRows = db.Execute(strSql,
                new
                {
                    IdDenuncia = entity.idDenuncia,
                    IdUsuario = entity.idUsuario,
                    Activo = entity.Activo
                }
                );

                return affectedRows;
            }
        }
        public int Update(string conn, DenunciaInvolucradoInputDto entity)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "update DenunciaInvolucrado set IdDenuncia=@IdDenuncia, IdUsuario=@IdUsuario, Activo=@Activo where IdDenunciaInvolucrado = @IdInvolucrado";
                var affectedRows = db.Execute(strSql,
                    new
                    {
                        IdDocumentoDenuncia = entity.IdDenunciaInvolucrado,
                        IdDenuncia = entity.idDenuncia,
                        IdUsuario = entity.idUsuario,
                        Activo = entity.Activo
                    }
                );

                return affectedRows;
            }
        }
        public int Delete(string conn, decimal IdDenunciaInvolucrado)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "Delete DenunciaInvolucrado where IdDenunciaInvolucrado = @IdDenunciaInvolucrado";
                var affectedRows = db.Execute(strSql,
                    new
                    {
                        IdDenunciaInvolucrado = IdDenunciaInvolucrado
                    }
                );

                return affectedRows;
            }
        }
        public int DeleteByIdDenuncia(string conn, decimal IdDenucia)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "Delete DenunciaInvolucrado where IdDenuncia = @IdDenuncia";
                var affectedRows = db.Execute(strSql,
                    new
                    {
                        IdDenuncia = IdDenucia
                    }
                );

                return affectedRows;
            }
        }
    }
}
