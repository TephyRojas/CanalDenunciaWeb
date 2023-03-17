using CanalDenunciaServicio.Data.Package.ModificacionPasswordPkg;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CanalDenunciaServicio.Data.Package.UsuarioPkg
{
    public class UsuarioService
    {
		public UsuarioLoginOutputDto SelectLogin(string conn, UsuarioLoginInputDto entity)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select top 1" +
							"		IdUsuario" +
							"		, Nombre + ' ' + PrimerApellido Usuario" +
							"		, Rut" +
							"		, Email" +
							"       ,isnull(PasswordTemporal,0) PasswordTemporal" +
							"		, isnull(PasswordBloqueada, 0) PasswordBloqueada" +
							"	   ,isnull(CONVERT(varchar,FechaBloqueo,103),'1900-01-01') FechaBloqueo" +
							"	   ,isnull(CONVERT(varchar,FechaCambioPassword,103),'1900-01-01') FechaCambioPassword" +
							"		,(select COUNT(a.idRol) CantidadRol from UsuarioRol a inner join Usuario b on a.IdUsuario = b.IdUsuario where b.Rut = '"+entity.Rut+"' and a.Activo = 1 and b.Activo = 1 ) as CantidadRol " +
							"	from" +
							"		Usuario " +
							"	where" +
							"		activo=1 "+
							"	and Rut = '"+entity.Rut + "'"+
							"	and Password = '"+entity.Password+"'";
							

				return (UsuarioLoginOutputDto)db.Query<UsuarioLoginOutputDto>
					(strSql)
					.SingleOrDefault();
			}
		}

		public UsuarioLoginOutputDto SelectAnonimo(string conn)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select top 1" +
							"		IdUsuario" +
							"		, Nombre + ' ' + PrimerApellido Usuario" +
							"		, Email" +
							"	from" +
							"		Usuario " +
							"	where" +
							"		activo=1 " +
							"	and idUsuario = 1";
							


				return (UsuarioLoginOutputDto)db.Query<UsuarioLoginOutputDto>
					(strSql)
					.SingleOrDefault();
			}
		}

		public UsuarioOutputDto SelectUsuarioByIdUsuario(string conn, int id)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select top 1" +
							"		a.IdUsuario" +
							"		, a.Nombre + ' ' + a.PrimerApellido Nombre" +
							"		,   e.Nombre Sede " +
							"	, f.Nombre Departamento " +
							"	from Usuario a " +
							"	inner join UsuarioRol b on a.IdUsuario = b.IdUsuario " +
							"	inner join Rol c on b.IdRol = c.IdRol " +
							"	inner join DepartamentoSede d on a.IdDepartamentoSede = d.IdDepartamentoSede " +
							"	inner join Sede e on d.IdSede = e.IdSede " +
							"	inner join Departamento f on d.IdDepartamento = f.IdDepartamento " +
							"	where a.IdUsuario = " + id +" and a.activo = 1 and b.activo = 1 ";	
						

				return (UsuarioOutputDto)db.Query<UsuarioOutputDto>
					(strSql)
					.SingleOrDefault();
			}
		}

		public UsuarioLoginOutputDto SelectUsuarioByRut(string conn, string rut)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select top 1" +
							"		IdUsuario" +
							"		, Nombre + ' ' + PrimerApellido Usuario" +
							"		, Rut" +
							"		, Email" +
							"       ,isnull(0,PasswordTemporal) PasswordTemporal" +
							"	from" +
							"		Usuario " +
							"	where" +
							"		activo=1 " +
							"	and Rut = '" + rut + "'";


				return (UsuarioLoginOutputDto)db.Query<UsuarioLoginOutputDto>
					(strSql)
					.SingleOrDefault();
			}
		}

		public int  SelectUsuarioBackup (string conn, int idUsuario)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select ISNULL(EsBackup, 0) from UsuarioRol where IdUsuario = "+idUsuario;


				return (int)db.Query<int>
					(strSql)
					.SingleOrDefault();
			}
		}

		public UsuarioLoginOutputDto SelectUsuarioById(string conn, int id)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select top 1" +
							"		Nombre + ' ' + PrimerApellido Usuario" +
							"		, Rut" +
							"		, Email" +
							"		, Telefono "+
							"	from" +
							"		Usuario " +
							"	where" +
							"		activo=1 " +
							"	and IdUsuario = '" + id + "'";


				return (UsuarioLoginOutputDto)db.Query<UsuarioLoginOutputDto>
					(strSql)
					.SingleOrDefault();
			}
		}

		public UsuarioDatosOutputDto SelectDatosByIdDenuncia(string conn, int idDenuncia)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select a.Nombre, b.Denuncia," +
					" case (select IdUsuarioDenuncia from Denuncia where idDenuncia = "+idDenuncia+") when 1 then  (select top 1 email from DenunciaCorreoAnonimo where idDenuncia="+idDenuncia+") else a.Email end Email , " +
					"c.Nombre TipoDelito " +
								" from Usuario a inner join Denuncia b on b.IdUsuarioDenuncia = a.IdUsuario " +
								" inner join TipoDelito c on b.IdTipoDelito = c.IdTipoDelito " +
								" where b.IdDenuncia = " + idDenuncia;

				return (UsuarioDatosOutputDto)db.Query<UsuarioDatosOutputDto>
					(strSql)
					.SingleOrDefault();
			}
		}

		public List<UsuarioEmailOutputDto> SelectEmailByIdRolList(string conn, int IdRol)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select c.Email, b.IdUsuario " +
								" from Rol a inner join UsuarioRol b on a.IdRol = b.IdRol " +
								" inner join Usuario c on b.IdUsuario = c.IdUsuario "+
								" where a.IdRol = "+IdRol +" and b.activo = 1 and c.activo = 1";

				return (List<UsuarioEmailOutputDto>)db.Query<UsuarioEmailOutputDto>
					(strSql)
					.ToList();
			}
		}

		public List<UsuarioEmailOutputDto> SelectEmailByBackUp(string conn)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select a.IdUsuario, a.Email from usuario a " +
								" inner join UsuarioRol b on a.idUsuario = b.IdUsuario where b.esbackup = 1 and a.activo = 1 ";

				return (List<UsuarioEmailOutputDto>)db.Query<UsuarioEmailOutputDto>
					(strSql)
					.ToList();
			}
		}

		public int UpdateContrasenaTemporal(string conn, UsuarioContrasenaTemporalInputDto entity)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "update Usuario set Password=@Password, PasswordTemporal=@PasswordTemporal, FechaCambioPassword=@FechaCambio where IdUsuario = @IdUsuario";
				var affectedRows = db.Execute(strSql,
					new
					{
						Password = entity.contrasenaTemporal,
						IdUsuario = entity.idUsuario,
						PasswordTemporal = entity.temporal,
						FechaCambio = entity.FechaCambio
					}
				) ;

				return affectedRows;
			}
		}

		public int UpdateBloquearContrasena(string conn, UsuarioBloqueoPassowordInputDto entity)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "update Usuario set PasswordBloqueada=1, FechaBloqueo=@FechaBloqueo where IdUsuario = @IdUsuario";
				var affectedRows = db.Execute(strSql,
					new
					{
						FechaBloqueo = entity.FechaBloqueo,
						IdUsuario = entity.IdUsuario
					}
				);

				return affectedRows;
			}
		}

		public int UpdateDatosPersonales(string conn, UsuarioDatosPersonalesInputDto entity)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "update Usuario set Telefono=@Telefono, Email=@Email where IdUsuario = @IdUsuario";
				var affectedRows = db.Execute(strSql,
					new
					{
						Telefono = entity.Telefono,
						Email = entity.Email,
						IdUsuario = entity.IdUsuario
					}
				);

				return affectedRows;
			}
		}

		public ModificacionPasswordOutputDto SelectCambioPswById(string conn, int id)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select top 1" +
							"		a.IdUsuario, " +
							"		a.Password" +							
							"	from Usuario a " +							
							"	where a.IdUsuario = " + id;


				return (ModificacionPasswordOutputDto)db.Query<ModificacionPasswordOutputDto>
					(strSql)
					.SingleOrDefault();
			}
		}

		public string Insert(string conn, UsuarioInputDto entity, int ultimo = 0)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "Sp_InsertUpdateUsuario";
				var rut = db.QuerySingle<string>(strSql,
				new
				{
					Rut = entity.Rut,
					Nombre = entity.Nombre,
					PrimerApellido = entity.PrimerApellido,
					SegundoApellido = entity.SegundoApellido,
					FechaIngresoContrato = entity.FechaIngresoContrato,
					FechaFinContrato = entity.FechaFinContrato,
					IdDepartamentoSede = entity.idDepartamentoSede,
					ultimo = ultimo
				},
				commandType: CommandType.StoredProcedure
				);

				return rut;
			}
		}
	}
}
