using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.UsuarioRolPkg
{
    public class UsuarioRolService
    {
		public List<UsuarioRolListDto> SelectUsuarioRolByIdUsuario(string conn, int idUsuario)
		
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select a.IdRol, b.Nombre from UsuarioRol a inner join Rol b on a.IdRol = b.IdRol and b.Activo = 1 inner join Usuario c on a.IdUsuario = c.IdUsuario and c.Activo = 1 where a.IdUsuario = " + idUsuario;
				return (List<UsuarioRolListDto>)db.Query<UsuarioRolListDto>
					(strSql)
					.ToList();
			}
		}

		public List<UsuarioRolListDto> SelectUsuarioRolByIdRol(string conn, string rol)

		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select a.IdRol, b.Nombre, a.IdUsuario from UsuarioRol a inner join Rol b on a.IdRol = b.IdRol and b.Activo = 1 inner join Usuario c on a.IdUsuario = c.IdUsuario and c.Activo = 1 where a.IdRol in ( " + rol+")";
				return (List<UsuarioRolListDto>)db.Query<UsuarioRolListDto>
					(strSql)
					.ToList();
			}
		}

		public List<UsuarioRolListDto> SelectUsuarioRolByBackup(string conn)

		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select a.IdRol, b.Nombre, a.IdUsuario " +
								" from UsuarioRol a inner join Rol b on a.IdRol = b.IdRol and b.Activo = 1 " +
								" inner join Usuario c on a.IdUsuario = c.IdUsuario and c.Activo = 1 " +
								" where a.EsBackup = 1 ";
				return (List<UsuarioRolListDto>)db.Query<UsuarioRolListDto>
					(strSql)
					.ToList();
			}
		}
	}
}
