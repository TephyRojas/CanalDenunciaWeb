using CanalDenunciaServicio.Data.Package.DenunciaInvolucradoPkg;
using CanalDenunciaServicio.Data.Package.DenunciaPkg;
using CanalDenunciaServicio.Helper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CanalDenunciaServicio.Controllers
{

    public class DenunciaController : ApiController
    {
        Util util = new Util();
        DenunciaService denunciaService = new DenunciaService();
        DenunciaInvolucradoService denunciaInvolucradoService = new DenunciaInvolucradoService();
        // GET: api/Denuncia/DenunciaList
        /// <summary>
        /// Obtener todos los Datos
        /// </summary>
        /// <remarks>
        /// Metodo para obtener todos los datos Denuncia
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpGet]
        public HttpResponseMessage DenunciaList(string denuncia = "", string idSede = "", string idDepartamento = "", string fechaInicio = "", string fechaFin = "", string inicioOcurrencia = "", string finOcurrencia = "", string idTipoDelito = "", string idEstado = "", string idUsuario = "", string idRol="0")
        {
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    var result = denunciaService.SelectList(util.Conn, denuncia, idSede, idDepartamento, fechaInicio, fechaFin, inicioOcurrencia, finOcurrencia, idTipoDelito, idEstado, idUsuario,idRol);
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

        // GET: api/Denuncia/DenunciaReporteList
        /// <summary>
        /// Obtener todos los Datos
        /// </summary>
        /// <remarks>
        /// Metodo para obtener todos los datos Denuncia
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpGet]
        public HttpResponseMessage DenunciaReporteList(string denuncia = "", string idSede = "", string idDepartamento = "", string fechaInicio = "", string fechaFin = "", string inicioOcurrencia = "", string finOcurrencia = "", string idTipoDelito = "", string idEstado = "", string idUsuario = "")
        {
            try
            {
                DenunciaInvolucradoOutputDto denunciaInvolucradoOutputDto = new DenunciaInvolucradoOutputDto();
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    var result = denunciaService.SelectReporteList(util.Conn, denuncia, idSede, idDepartamento, fechaInicio, fechaFin, inicioOcurrencia, finOcurrencia, idTipoDelito, idEstado, idUsuario);
                    if (result != null)
                    {
                        if (result.Count > 0)
                        {
                            foreach (var dato in result)
                            {
                                string involucrados = "";
                                //Obtener String de los denunciados
                                var resultInvolucrado = denunciaService.SelectListInvolucrado(util.Conn, dato.IdDenuncia);
                                if (resultInvolucrado != null)
                                {
                                    if (resultInvolucrado.Count > 0)
                                    {

                                        foreach (var involucrado in resultInvolucrado)
                                        {
                                            involucrados = involucrados + involucrado.Nombre + "-";
                                        }

                                        involucrados = involucrados.TrimEnd('-');

                                    }
                                }

                                dato.Denunciados = involucrados;
                            }

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


        // GET: api/Denuncia/DenunciaDetalleByIdDenuncia
        /// <summary>
        /// Obtener todos los Datos
        /// </summary>
        /// <remarks>
        /// Metodo para obtener todos los datos Denuncia por IdDenuncia
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpGet]
        public HttpResponseMessage DenunciaDetalleByIdDenuncia(int IdDenuncia)
        {
            try
            {
                List<DenunciaInvolucrado> lstInvolucrados = new List<DenunciaInvolucrado>();
                List<DenunciaComentario> lstComentarios = new List<DenunciaComentario>();
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    var result = denunciaService.SelectDetalleByIdDenuncia(util.Conn, IdDenuncia);
                    if (result != null)
                    {
                        var resultInvolucrado = denunciaService.SelectListInvolucrado(util.Conn, IdDenuncia);
                        if (resultInvolucrado != null)
                        {
                            if (resultInvolucrado.Count > 0)
                            {

                                foreach (var involucrado in resultInvolucrado)
                                {
                                    DenunciaInvolucrado denunciado = new DenunciaInvolucrado
                                    {
                                        IdRol = involucrado.IdRol,
                                        IdSede = involucrado.IdSede,
                                        IdUsuario = involucrado.IdUsuario,
                                        Departamento = involucrado.Departamento,
                                        Sede = involucrado.Sede,
                                        Nombre = involucrado.Nombre
                                    };
                                    lstInvolucrados.Add(denunciado);
                                }
                                result.Involucrados = lstInvolucrados;
                            }
                        }

                        var resultComentarios = denunciaService.SelectListComentario(util.Conn, IdDenuncia);
                        if (resultComentarios != null)
                        {
                            if (resultComentarios.Count > 0)
                            {

                                foreach (var comentario in resultComentarios)
                                {
                                    DenunciaComentario comentarios = new DenunciaComentario
                                    {
                                        IdDenunciaComentario = comentario.IdDenunciaComentario,
                                        Usuario = comentario.Usuario,
                                        Comentario = comentario.Comentario,
                                        Fecha = comentario.Fecha

                                    };
                                    lstComentarios.Add(comentarios);
                                }
                                result.Comentarios = lstComentarios;
                            }
                        }
                        return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(1, "OK", validaToken, result));
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

        // GET: api/Denuncia/DenunciaDetalleByIdDenuncia
        /// <summary>
        /// Obtener todos los Datos
        /// </summary>
        /// <remarks>
        /// Metodo para obtener todos los datos Denuncia por IdDenuncia
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpGet]
        public HttpResponseMessage DenunciaDetalleByDenuncia(string Denuncia)
        {
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    var result = denunciaService.SelectDetalleByDenuncia(util.Conn, Denuncia);
                    if (result != null)
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
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                util.logSistema(ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, util.respuestaGenerica(0, util.Error, ""));
            }
        }

        // POST: api/Denuncia/PostDenuncia
        /// <summary>
        /// Insertar Datos
        /// </summary>
        /// <remarks>
        /// Metodo para insertar datos en la tabla Denuncia
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpPost]
        public HttpResponseMessage PostDenuncia(DenunciaInputDto entity)
        {
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    if (ModelState.IsValid)
                    {
                        var result = denunciaService.Insert(util.Conn, entity);
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
        [HttpPost]
        public HttpResponseMessage PutDenuncia(DenunciaInputDto entity)
        {
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    if (ModelState.IsValid)
                    {
                        var result = denunciaService.Update(util.Conn, entity);
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

        // POST: api/Denuncia/PutDenunciaEstado
        /// <summary>
        /// Actualizar Datos
        /// </summary>
        /// <remarks>
        /// Metodo para actualizar el estado en la tabla Denuncia
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpPost]
        public HttpResponseMessage PutDenunciaEstado(DenunciaEstadoInputDto entity)
        {
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    if (ModelState.IsValid)
                    {
                        var result = denunciaService.UpdateEstado(util.Conn, entity);
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

        // GET: api/Denuncia/DeleteDenuncia
        /// <summary>
        /// Eliminar un registro
        /// </summary>
        /// <remarks>
        /// Metodo para eliminar un registro de la tabla Denuncia
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpGet]
        public HttpResponseMessage DeleteDenuncia(decimal id)
        {
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    var result = denunciaService.Delete(util.Conn, id);
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
