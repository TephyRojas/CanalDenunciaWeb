using CanalDenunciaServicio.Data.Package.DepartamentoPkg;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.TipoDelitoPkg
{
    public class TipoDelitoService
    {
		public List<TipoDelitoComboboxOutputDto> SelectCombobox(string conn)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select IdTipoDelito, Nombre  from TipoDelito where Activo = 1";
				return (List<TipoDelitoComboboxOutputDto>)db.Query<TipoDelitoComboboxOutputDto>
					(strSql)
					.ToList();
			}
		}

		public TipoDelitoDescripcionOutputDto SelectDescripcionByIdTipoDelito(string conn, string idTipoDelito)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select top 1" +
							"	Descripcion" +
							"	from " +
							"		TipoDelito " +
							"	where" +
							"		activo=1 " +
							"	and IdTipoDelito = '" + idTipoDelito + "'";


				return (TipoDelitoDescripcionOutputDto)db.Query<TipoDelitoDescripcionOutputDto>
					(strSql)
					.SingleOrDefault();
			}
		}

		public TipoDelitoNombreOutputDto SelectNombreByIdTipoDelito(string conn, string idTipoDelito)
		{
			using (IDbConnection db = new SqlConnection(conn))
			{
				string strSql = "select top 1" +
							"	Nombre" +
							"	from " +
							"		TipoDelito " +
							"	where" +
							"		activo=1 " +
							"	and IdTipoDelito = '" + idTipoDelito + "'";


				return (TipoDelitoNombreOutputDto)db.Query<TipoDelitoNombreOutputDto>
					(strSql)
					.SingleOrDefault();
			}
		}
	}

}
