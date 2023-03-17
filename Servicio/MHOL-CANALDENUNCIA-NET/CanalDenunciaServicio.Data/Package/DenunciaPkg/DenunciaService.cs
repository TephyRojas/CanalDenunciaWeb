using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaPkg
{
    public class DenunciaService
    {

        public List<DenunciaListOutputDto> SelectList(string conn, string denuncia, string idSede, string idDepartamento, string fechaInicio, string fechaFin, string inicioOcurrencia, string finOcurrencia, string idTipoDelito, string idEstado, string idUsuario, string idRol)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select a.idDenuncia, a.Denuncia " +
                              "  ,CONVERT(varchar,a.FechaIngreso,103) FechaIngreso " +
                              "  , b.Nombre Sede " +
                              "  , c.Nombre TipoDelito " +
                              "   , d.Nombre Estado " +
                              "  from " +
                              "  Denuncia a " +
                              "  inner join DepartamentoSede e on a.IdDepartamentoSede = e.IdDepartamentoSede " +
                              "  inner join Sede b on e.IdSede = b.IdSede " +
                              "  inner join TipoDelito c on a.IdTipoDelito = c.IdTipoDelito " +
                              "  inner join EstadoDenuncia d on a.IdEstadoDenuncia = d.IdEstadoDenuncia "+
                              "  left join DenunciaOficialCumplimiento F on a.IdDenuncia = f.IdDenuncia ";
                if (denuncia.Length > 0 || idSede.Length > 0 || idDepartamento.Length > 0 || fechaInicio.Length > 0 || fechaFin.Length > 0 || idTipoDelito.Length > 0 || idEstado.Length > 0 || idRol !="")
                {
                    strSql += " where 1=1 ";
                }

                if (denuncia.Length > 0)
                {
                    strSql += " and a.denuncia like '%" + denuncia + "%'";
                }

                if (idSede.Length > 0)
                {
                    strSql += " and e.idSede=" + idSede + "";
                }

                if (idDepartamento.Length > 0)
                {
                    strSql += " and e.idDepartamento='" + idDepartamento + "'";
                }

                if (fechaInicio.Length > 0 && fechaFin.Length > 0)
                {
                    strSql += " and (Convert(varchar,a.FechaIngreso,20) >= '" + fechaInicio + " 00:00:00' and Convert(varchar,a.FechaIngreso,20) <= '" + fechaFin + " 23:59:59')";
                }

                if (inicioOcurrencia.Length > 0 && finOcurrencia.Length > 0)
                {
                    strSql += " and (Convert(varchar,a.FechaInicio,20) >= '" + inicioOcurrencia + " 00:00:00' and Convert(varchar,a.FechaFin,20) <= '" + finOcurrencia + " 23:59:59')";
                }

                if (idTipoDelito.Length > 0)
                {
                    strSql += " and a.idTipoDelito=" + idTipoDelito + "";
                }

                if (idEstado.Length > 0)
                {
                    strSql += " and a.idEstadoDenuncia=" + idEstado + "";
                }
               
                if (idRol == "3")
                {
                    if (idUsuario.Length > 0)
                    {
                        strSql += " and a.idUsuarioDenuncia  = " + idUsuario;
                    }
                }
                else if(idRol == "4")
                {
                    strSql += " and a.IdDenuncia in (select IdDenuncia from DenunciaOficialCumplimiento)";
                   
                }
                else 
                {
                    if (idUsuario.Length > 0)
                    {
                        strSql += " and a.IdDenuncia not in (select IdDenuncia from DenunciaInvolucrado where IdUsuario = " + idUsuario + ")";
                    }
                }


                return (List<DenunciaListOutputDto>)db.Query<DenunciaListOutputDto>
                    (strSql)
                    .ToList();
            }
        }

        public List<DenunciaReporteListOutputDto> SelectReporteList(string conn, string denuncia, string idSede, string idDepartamento, string fechaInicio, string fechaFin, string inicioOcurrencia, string finOcurrencia, string idTipoDelito, string idEstado, string idUsuario)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select a.idDenuncia, a.Denuncia " +
                                " , f.Nombre + ' ' + f.PrimerApellido Denunciante " +
                                " ,CONVERT(varchar, a.FechaIngreso, 103) FechaIngreso " +
                                " ,CONVERT(varchar, a.FechaInicio, 103) FechaDesde " +
                                " ,CONVERT(varchar, a.FechaFin, 103) FechaHasta " +
                                " , b.Nombre Sede " +
                                " , g.Nombre Departamento " +
                                " , c.Nombre TipoDelito " +
                                " , d.Nombre Estado " +
                                " , DATEDIFF(DAY, FechaIngreso, GETDATE()) Diferencia " +
                                "    from " +
                                " Denuncia a " +
                                " inner join DepartamentoSede e on a.IdDepartamentoSede = e.IdDepartamentoSede " +
                                " inner join Sede b on e.IdSede = b.IdSede " +
                                " inner join TipoDelito c on a.IdTipoDelito = c.IdTipoDelito " +
                                " inner join EstadoDenuncia d on a.IdEstadoDenuncia = d.IdEstadoDenuncia " +
                                " inner join Usuario f on a.IdUsuarioDenuncia = f.idUsuario " +
                                " inner join Departamento g on e.IdDepartamento = g.IdDepartamento ";
                if (denuncia.Length > 0 || idSede.Length > 0 || idDepartamento.Length > 0 || fechaInicio.Length > 0 || fechaFin.Length > 0 || idTipoDelito.Length > 0 || idEstado.Length > 0)
                {
                    strSql += " where 1=1 ";
                }

                if (denuncia.Length > 0)
                {
                    strSql += " and a.denuncia like '%" + denuncia + "%'";
                }

                if (idSede.Length > 0)
                {
                    strSql += " and e.idSede=" + idSede + "";
                }

                if (idDepartamento.Length > 0)
                {
                    strSql += " and e.idDepartamento='" + idDepartamento + "'";
                }

                if (fechaInicio.Length > 0 && fechaFin.Length > 0)
                {
                    strSql += " and (Convert(varchar,a.FechaIngreso,20) >= '" + fechaInicio + " 00:00:00' and Convert(varchar,a.FechaIngreso,20) <= '" + fechaFin + " 23:59:59')";
                }

                if (inicioOcurrencia.Length > 0 && finOcurrencia.Length > 0)
                {
                    strSql += " and (Convert(varchar,a.FechaInicio,20) >= '" + inicioOcurrencia + " 00:00:00' and Convert(varchar,a.FechaFin,20) <= '" + finOcurrencia + " 23:59:59')";
                }

                if (idTipoDelito.Length > 0)
                {
                    strSql += " and a.idTipoDelito=" + idTipoDelito + "";
                }

                if (idEstado.Length > 0)
                {
                    strSql += " and a.idEstado=" + idEstado + "";
                }

                if (idUsuario.Length > 0)
                {
                    strSql += " and a.idUsuarioDenuncia  = " + idUsuario;
                }


                return (List<DenunciaReporteListOutputDto>)db.Query<DenunciaReporteListOutputDto>
                    (strSql)
                    .ToList();
            }
        }

        public DenunciaDetalleDto SelectDetalleByIdDenuncia(string conn, int idDenuncia)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select a.Denuncia, a.IdDepartamentoSede,a.IdTipoDelito, e.Descripcion, " +
                                " CONVERT(VARCHAR, a.FechaInicio, 103)FechaInicio,CONVERT(VARCHAR, a.FechaFin, 103)FechaFin " +
                                //" ISNULL(a.Descripcion,'') Descripcion" +
                                ",a.IdEstadoDenuncia,c.IdSede, d.PermiteModificacion " +
                                " from Denuncia a " +
                                " inner join DepartamentoSede b on a.IdDepartamentoSede = b.IdDepartamentoSede " +
                                " inner join Sede c on b.IdSede = c.IdSede " +
                                " inner join EstadoDenuncia d on a.IdEstadoDenuncia = d.IdEstadoDenuncia "+
                                " inner join TipoDelito e on a.IdTipoDelito = e.IdTipoDelito "+
                                " where a.IdDenuncia = " +idDenuncia;
                return (DenunciaDetalleDto)db.Query<DenunciaDetalleDto>
                    (strSql)
                    .SingleOrDefault();
            }
        }

        public DenunciaIdOutputDto SelectDetalleByDenuncia(string conn, string denuncia)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select a.IdDenuncia from Denuncia a  where Denuncia = '" + denuncia+"'";
                return (DenunciaIdOutputDto)db.Query<DenunciaIdOutputDto>
                    (strSql)
                    .SingleOrDefault();
            }
        }
        public List<DenunciaInvolucrado> SelectListInvolucrado(string conn, int idDenuncia)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "Select a.idUsuario, b.Nombre +' '+b.PrimerApellido Nombre, d.idRol, g.idSede, h.Nombre Departamento, g.Nombre Sede " +
                                " From DenunciaInvolucrado a " +
                                " inner join Usuario b on a.idUsuario = b.IdUsuario " +
                                " inner join UsuarioRol c on b.idUsuario = c.IdUsuario " +
                                " inner join Rol d on c.idRol = d.idRol " + 
                                " inner join Denuncia e on e.iddenuncia = a.idDenuncia " +
                                " inner join DepartamentoSede f on f.idDepartamentoSede = e.IdDepartamentoSede " +
                                " inner join Sede g on g.idSede = f.idSede " +
                                " inner join Departamento h on h.IdDepartamento = f.idDepartamento "+
                                " where a.idDenuncia = "+idDenuncia;
               
                return (List<DenunciaInvolucrado>)db.Query<DenunciaInvolucrado>
                    (strSql)
                    .ToList();
            }
        }

        public List<DenunciaComentario> SelectListComentario(string conn, int idDenuncia)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "select b.Nombre+' '+b.PrimerApellido as Usuario,a.Comentario, a.IdDenunciaComentario, CONVERT(VARCHAR, a.Fecha, 103)Fecha " +
                                " from DenunciaComentario a " +
                                " inner join Usuario b on a.Usuario = b.IdUsuario " +
                                " where a.idDenuncia = " + idDenuncia;

                return (List<DenunciaComentario>)db.Query<DenunciaComentario>
                    (strSql)
                    .ToList();
            }
        }

        public int Insert(string conn, DenunciaInputDto entity)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "insert into " +
                    "Denuncia (IdDepartamentoSede,IdTipoDelito,IdEstadoDenuncia,Denuncia,FechaIngreso,FechaInicio,FechaFin, IdUsuarioDenuncia,Activo) " +
                    "values(@IdDepartamentoSede,@IdTipoDelito,@IdEstadoDenuncia,@Denuncia,@FechaIngreso,@FechaInicio,@FechaFin,@IdUsuario,@Activo)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
                var affectedRows = db.QuerySingle<int>(strSql,
                new
                {
                    IdDepartamentoSede = entity.IdDepartamentoSede,
                    IdTipoDelito = entity.IdTipoDelito,
                    IdEstadoDenuncia = entity.IdEstadoDenuncia,
                    Denuncia = entity.Denuncia,
                    FechaIngreso = entity.FechaIngreso,
                    FechaInicio = entity.FechaInicio,
                    FechaFin = entity.FechaFin,
                    //Descripcion = entity.Descripcion,
                    IdUsuario = entity.IdUsuario,
                    Activo = entity.Activo
                }
                );

                return affectedRows;
            }
        }

        public int Update(string conn, DenunciaInputDto entity)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "update Denuncia set IdDepartamentoSede=@IdDepartamentoSede, " +
                    "IdTipoDelito=@IdTipoDelito, " +
                   // "IdEstadoDenuncia = @IdEstadoDenuncia," +
                    " FechaIngreso = @FechaIngreso, " +
                    "FechaInicio = @FechaInicio, " +
                    "FechaFin = @FechaFin " +
                    " where IdDenuncia = @IdDenuncia";
                var affectedRows = db.Execute(strSql,
                    new
                    {
                        IdDenuncia = entity.IdDenuncia,
                        IdDepartamentoSede = entity.IdDepartamentoSede,
                        IdTipoDelito = entity.IdTipoDelito,
                        IdEstadoDenuncia = entity.IdEstadoDenuncia,
                        FechaIngreso = entity.FechaIngreso,
                        FechaInicio = entity.FechaInicio,
                        FechaFin = entity.FechaFin,
                        //Descripcion = entity.Descripcion,
                        Activo = entity.Activo
                    }
                );

                return affectedRows;
            }
        }
        public int UpdateEstado(string conn, DenunciaEstadoInputDto entity)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "update Denuncia set  " +
                    "IdEstadoDenuncia = @IdEstadoDenuncia " +
                    " where IdDenuncia = @IdDenuncia";
                var affectedRows = db.Execute(strSql,
                    new
                    {
                        IdDenuncia = entity.IdDenuncia,
                        IdEstadoDenuncia = entity.IdEstadoDenuncia
                    }
                );

                return affectedRows;
            }

          

        }
        public int Delete(string conn, decimal IdDenuncia)
        {
            using (IDbConnection db = new SqlConnection(conn))
            {
                string strSql = "update Denuncia set activo=0 where IdDenuncia = @IdDenuncia";
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
