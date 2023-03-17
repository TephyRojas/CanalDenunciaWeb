using CanalDenunciaServicio.Data.Package.DepartamentoSedePkg;
using CanalDenunciaServicio.Helper;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CanalDenunciaServicio.Controllers
{
    public class DepartamentoSedeController : ApiController
    {
        Util util = new Util();
        DepartamentoSedeService departamentoSedeService = new DepartamentoSedeService();
        // GET: api/Sede/DepartamentoSedeCombobox
        /// <summary>
        /// Obtener todos los Datos
        /// </summary>
        /// <remarks>
        /// Metodo para obtener todos los datos Departamento Sede
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpGet]
        public HttpResponseMessage DepartamentoSedeCombobox(int idSede)
        {
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    var result = departamentoSedeService.SelectCombobox(util.Conn, idSede);
                    if (result != null)
                    {
                        if (result.Count > 0)
                        {
                            return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(1, "OK", validaToken, result));
                        }
                        else
                        {
                            return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(204, util.NoData, validaToken, result));
                        }
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.NotImplemented, util.respuestaGenerica(0, util.NoData, validaToken, result));
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                util.logSistema(ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, util.respuestaGenerica(0, util.Error, ""));
            }
        }

        // GET: api/DepartamentoSede/DepartamentoSedeExist
        /// <summary>
        /// Obtener todos los Datos por nombre departamento y sede
        /// </summary>
        /// <remarks>
        /// Metodo para obtener todos los datos DenunciaInvolucrado según nombre departamento y sede
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpGet]
        public HttpResponseMessage DepartamentoSedeExist( string departamento, string sede)
        {
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    var result = departamentoSedeService.SelectByDepartamentoSede(util.Conn, departamento,sede);
                    if (result != 0)
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(1, "OK", validaToken, result));
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(204, util.NoData, validaToken, result));
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                util.logSistema(ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, util.respuestaGenerica(0, util.Error, ""));
            }
        }
    }
}
