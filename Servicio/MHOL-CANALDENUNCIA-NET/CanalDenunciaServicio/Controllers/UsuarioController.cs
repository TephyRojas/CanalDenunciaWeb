using CanalDenunciaServicio.Data.Package.UsuarioPkg;
using CanalDenunciaServicio.Data.Package.UsuarioRolPkg;
using CanalDenunciaServicio.Data.Package.ModificacionPasswordPkg;
using CanalDenunciaServicio.Helper;
using CanalDenunciaServicio.Security;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CanalDenunciaServicio.Controllers
{

    public class UsuarioController : ApiController
    {
        private Util util = new Util();
        private UsuarioService usuarioService = new UsuarioService();
        private UsuarioRolService usuarioRolService = new UsuarioRolService();
        private ModificacionPasswordService modificacionPasswordService = new ModificacionPasswordService();
        // POST: api/Usuario/UsuarioLogin
        /// <summary>
        /// Insertar Datos
        /// </summary>
        /// <remarks>
        /// Metodo para insertar datos en la tabla Usuario
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpPost]
        public HttpResponseMessage UsuarioLogin(UsuarioLoginInputDto entity)
        {
            List<UsuarioRolListDto> usuarioRolListDtos = new List<UsuarioRolListDto>();
            try
            {
                if (ModelState.IsValid)
                {
                    var result = usuarioService.SelectLogin(util.Conn, entity);
                    if (result != null)
                    {
                        var resultRol = usuarioRolService.SelectUsuarioRolByIdUsuario(util.Conn, Convert.ToInt32(result.IdUsuario));
                        if (resultRol != null)
                        {
                            foreach (var rol in resultRol)
                            {
                                usuarioRolListDtos.Add(rol);
                            }
                            result.IdRol = usuarioRolListDtos[0].IdRol;
                        }

                        var tokenOuput = TokenGenerator.GenerateTokenJwt(result.IdUsuario.ToString(), result.Email, result.Usuario, Convert.ToInt32(util.DuracionToken));

                        return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(1, "OK", tokenOuput, result));
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(204, util.NoData, "", result));
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, util.respuestaGenerica(0, util.NoData, ""));
                }
            }
            catch (Exception ex)
            {
                util.logSistema(ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, util.respuestaGenerica(0, util.Error, ""));
            }
        }

        // POST: api/Usuario/UsuarioLogin
        /// <summary>
        /// Insertar Datos
        /// </summary>
        /// <remarks>
        /// Metodo para insertar datos en la tabla Usuario
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpPost]
        public HttpResponseMessage UsuarioAnonimo()
        {
            List<UsuarioRolListDto> usuarioRolListDtos = new List<UsuarioRolListDto>();
            try
            {
                if (ModelState.IsValid)
                {
                    var result = usuarioService.SelectAnonimo(util.Conn);
                    if (result != null)
                    {
                        var tokenOuput = TokenGenerator.GenerateTokenJwt(result.IdUsuario.ToString(), result.Email, result.Usuario, Convert.ToInt32(util.DuracionToken));

                        return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(1, "OK", tokenOuput, result));
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(204, util.NoData, "", result));
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, util.respuestaGenerica(0, util.NoData, ""));
                }
            }
            catch (Exception ex)
            {
                util.logSistema(ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, util.respuestaGenerica(0, util.Error, ""));
            }
        }

        // GET: api/Usuario/UsuarioByIdUsuario
        /// <summary>
        /// Obtener Datos por RUT
        /// </summary>
        /// <remarks>
        /// Metodo para obtener todos los datos Usuario según RUT
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpGet]
        public HttpResponseMessage UsuarioByIdUsuario(int idUsuario)
        {
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    var result = usuarioService.SelectUsuarioByIdUsuario(util.Conn, idUsuario);
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
        // GET: api/Usuario/UsuarioByRut
        /// <summary>
        /// Obtener Datos por RUT
        /// </summary>
        /// <remarks>
        /// Metodo para obtener todos los datos Usuario según RUT
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpGet]
        public HttpResponseMessage UsuarioByRut(string rut)
        {
            try
            {              
                
                    var result = usuarioService.SelectUsuarioByRut(util.Conn, rut);
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

        // GET: api/Usuario/UsuarioById
        /// <summary>
        /// Obtener Datos por Id
        /// </summary>
        /// <remarks>
        /// Metodo para obtener todos los datos Usuario según Id
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpGet]
        public HttpResponseMessage UsuarioById(int id)
        {
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    var result = usuarioService.SelectUsuarioById(util.Conn, id);
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

        // GET: api/Usuario/UsuarioById
        /// <summary>
        /// Obtener Datos por Id
        /// </summary>
        /// <remarks>
        /// Metodo para obtener todos los datos Usuario según Id
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpGet]
        public HttpResponseMessage UsuarioEsBackup(int id)
        {
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    var result = usuarioService.SelectUsuarioBackup(util.Conn, id);
                    if (result >= 0)
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

        // POST: api/Usuario/PutUsuarioContrasenaTemporal
        /// <summary>
        /// Actualizar Datos
        /// </summary>
        /// <remarks>
        /// Metodo para actualizar Contrasena Temporal en la tabla Usuario
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpPost]
        public HttpResponseMessage PutUsuarioContrasenaTemporal(UsuarioContrasenaTemporalInputDto entity)
        {
           
            try
            {
                if (ModelState.IsValid)
                {
                    var resultContraseñaAntigua = usuarioService.SelectCambioPswById(util.Conn, Convert.ToInt32(entity.idUsuario));
                    if(resultContraseñaAntigua != null)
                    {
                        ModificacionPasswordInputDto modificacionPasswordInputDto = new ModificacionPasswordInputDto()
                        {
                            IdUsuario = Convert.ToInt32(entity.idUsuario),
                            Password = resultContraseñaAntigua.Password,
                            FechaCambio = entity.FechaCambio
                        };

                        var resultCambio = modificacionPasswordService.Insert(util.Conn, modificacionPasswordInputDto);
                        if(resultCambio>0)
                        {
                            var result = usuarioService.UpdateContrasenaTemporal(util.Conn, entity);


                            if (result > 0)
                            {
                                //var tokenOuput = TokenGenerator.GenerateTokenJwt(result.IdUsuario.ToString(), result.Email, result.Usuario, Convert.ToInt32(util.DuracionToken));

                                return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(1, "OK", "", result));
                            }                            
                        }                        
                    }

                    return this.Request.CreateResponse(HttpStatusCode.NotImplemented, util.respuestaGenerica(0, util.NoData, ""));
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, util.respuestaGenerica(0, util.NoData, ""));
                }

            }
            catch (Exception ex)
            {
                util.logSistema(ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, util.respuestaGenerica(0, util.Error, ""));
            }
        }
        // POST: api/Usuario/PutUsuarioBloqueoContrasena
        /// <summary>
        /// Actualizar Datos
        /// </summary>
        /// <remarks>
        /// Metodo para actualizar Bloqueo de Password en la tabla Usuario
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpPost]
        public HttpResponseMessage PutUsuarioBloqueoContrasena(UsuarioBloqueoPassowordInputDto entity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = usuarioService.UpdateBloquearContrasena(util.Conn, entity);


                    if (result > 0)
                    {
                        //var tokenOuput = TokenGenerator.GenerateTokenJwt(result.IdUsuario.ToString(), result.Email, result.Usuario, Convert.ToInt32(util.DuracionToken));

                        return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(1, "OK", "", result));
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.NotImplemented, util.respuestaGenerica(0, util.NoData, "", result));
                    }
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, util.respuestaGenerica(0, util.NoData, ""));
                }

            }
            catch (Exception ex)
            {
                util.logSistema(ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, util.respuestaGenerica(0, util.Error, ""));
            }
        }

        // POST: api/Usuario/PutUsuarioDatosPersonales
        /// <summary>
        /// Actualizar Datos
        /// </summary>
        /// <remarks>
        /// Metodo para actualizar Telefono y Email en la tabla Usuario
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpPost]
        public HttpResponseMessage PutUsuarioDatosPersonales(UsuarioDatosPersonalesInputDto entity)
        {
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    if (ModelState.IsValid)
                    {
                        var result = usuarioService.UpdateDatosPersonales(util.Conn, entity);
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


        // GET: api/DatosEnvioCorreo/Password by id
        /// <summary>
        /// Obtener todos los Datos
        /// </summary>
        /// <remarks>
        /// Metodo para obtener todos las contraseñas según id
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpGet]
        public HttpResponseMessage SelectListPasswordById(int id)
        {
            try
            {
               
                    var result = modificacionPasswordService.SelectPasswordByIdList(util.Conn, id);
                    if (result != null)
                    {
                        if (result.Count > 0)
                        {
                            return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(1, "OK", "", result));
                        }
                        else
                        {
                            return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(204, util.NoData, "", result));
                        }
                    }
                    else
                    {
                        return this.Request.CreateResponse(HttpStatusCode.NotImplemented, util.respuestaGenerica(0, util.NoData, "", result));
                    }
                
            }
            catch (Exception ex)
            {
                util.logSistema(ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, util.respuestaGenerica(0, util.Error, ""));
            }
        }

        // POST: api/Usuario/PostUsuario
        /// <summary>
        /// Insertar Datos
        /// </summary>
        /// <remarks>
        /// Metodo para insertar datos en la tabla Usuario
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /// <response code="200">OK. Obtener todos los datos.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        [HttpPost]
        public HttpResponseMessage PostUsuario(List<UsuarioInputDto> entity)
        {
            bool resultado = true;
            string rutError = "";
            int x = 1;
            int ultimo = 0;
            try
            {
                var validaToken = util.RenuevaToken(this.Request.Headers);
                if (validaToken != null)
                {
                    if (ModelState.IsValid)
                    {
                        
                        foreach(var item in entity)
                        {
                            if(x == entity.Count)
                            {
                                ultimo = 1;
                            }
                            var result = usuarioService.Insert(util.Conn, item,ultimo);
                            if(result != "")
                            {
                                resultado = true;
                            }
                            else
                            {
                                resultado = false;
                                rutError = item.Rut;
                                break;
                            }
                            x = x + 1;
                        }
                        if (resultado)
                        {
                            return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(1, "OK", validaToken, 1));
                        }
                        else
                        {
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, util.respuestaGenerica(0, util.NoData, validaToken, rutError));
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















        ////// POST: api/Usuario/UsuarioLogin
        /////// <summary>
        /////// Buscar Datos
        /////// </summary>
        /////// <remarks>
        /////// Metodo para buscar datos en la tabla Usuario
        /////// </remarks>
        /////// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>
        /////// <response code="200">OK. Obtener todos los datos.</response>
        /////// <response code="400">BadRequest. No se ha creado el objeto en la BD. Formato del objeto incorrecto.</response>
        ////[HttpPost]
        ////public HttpResponseMessage UsuarioLogin(UsuarioLoginInputDto entity)
        ////{
        ////    try
        ////    {

        ////        if (ModelState.IsValid)
        ////        {
        ////            var result = usuarioService.SelectLogin(util.Conn, entity);
        ////            if (result != null)
        ////            {

        ////                var tokenOuput = TokenGenerator.GenerateTokenJwt(result.IdUsuario.ToString(), result.Email, result.Usuario, Convert.ToInt32(util.DuracionToken));

        ////                return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(1, "OK", tokenOuput, result));
        ////            }
        ////            else
        ////            {
        ////                return this.Request.CreateResponse(HttpStatusCode.OK, util.respuestaGenerica(204, util.NoData, "", result));
        ////            }
        ////        }
        ////        else
        ////        {
        ////            return this.Request.CreateResponse(HttpStatusCode.BadRequest, util.respuestaGenerica(0, util.NoData, ""));
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        util.logSistema(ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
        ////        return this.Request.CreateResponse(HttpStatusCode.InternalServerError, util.respuestaGenerica(0, util.Error, ""));
        ////    }

        ////}

    }
}
