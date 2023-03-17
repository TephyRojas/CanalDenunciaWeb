using CanalDenunciaServicio.Data.Package.CorreoPkg;
using CanalDenunciaServicio.Helper;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CanalDenunciaServicio.Controllers
{
    public class CorreoController : ApiController
    {
        private Util util = new Util();
        private CorreoService correoService = new CorreoService();

        // GET: api/Correo/CorreoByIdentificador
        /// <summary>
        /// Obtener todos los Datos por Identificador
        /// </summary>
        /// <remarks>
        /// Metodo para obtener todos los datos Correo según Identificador
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpGet]
        public HttpResponseMessage CorreoByIdentificador(string identificador)
        {
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    var result = correoService.SelectCorreo(util.Conn, identificador);
                    if (result != null)
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

        // GET: api/Correo/CorreoByIdentificador
        /// <summary>
        /// Obtener todos los Datos por Identificador
        /// </summary>
        /// <remarks>
        /// Metodo para obtener todos los datos Correo según Identificador
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpGet]
        public HttpResponseMessage CorreoByIdentificadorFuera(string identificador)
        {
            try
            {
                
                    var result = correoService.SelectCorreo(util.Conn, identificador);
                    if (result != null)
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(1, "OK", "", result));
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(204, util.NoData, "", result));
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