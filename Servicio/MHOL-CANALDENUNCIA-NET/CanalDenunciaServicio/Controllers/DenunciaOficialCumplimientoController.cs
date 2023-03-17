﻿using CanalDenunciaServicio.Data.Package.DenunciaOficialCumplimientoPkg;
using CanalDenunciaServicio.Helper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CanalDenunciaServicio.Controllers
{
    public class DenunciaOficialCumplimientoController : ApiController
    {
        Util util = new Util();
        DenunciaOficialCumplimientoService denunciaOficialCumplimientoService = new DenunciaOficialCumplimientoService();
        // POST: api/DenunciaOficialCumplimiento/PostDenunciaOficialCumplimiento
        /// <summary>
        /// Insertar Datos
        /// </summary>
        /// <remarks>
        /// Metodo para insertar datos en la tabla DenunciaOficialCumplimiento
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpPost]
        public HttpResponseMessage PostDenuncia(DenunciaOficialCumplimientoInputDto entity)
        {
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    if (ModelState.IsValid)
                    {
                        var result = denunciaOficialCumplimientoService.Insert(util.Conn, entity);
                        if (result > 0)
                        {
                            return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(1, "OK", validaToken, result));
                        }
                        else
                        {
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, util.respuestaGenerica(0, util.NoData, validaToken, result));
                        }
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.BadRequest, util.respuestaGenerica(0, util.NoData, validaToken));
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
