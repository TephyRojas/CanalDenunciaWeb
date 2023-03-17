using CanalDenunciaWeb.Data.Package.CorreoPkg;
using CanalDenunciaWeb.Data.Package.DenunciaPkg;
using CanalDenunciaWeb.Data.Package.UsuarioPkg;
using CanalDenunciaWeb.Helper;
using CanalDenunciaWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using CanalDenunciaWeb.Data.Package.UsuarioRolPkg;
using CanalDenuncia.Data.Package.UsuarioPkg;

namespace CanalDenunciaWeb.Controllers
{
    public class LoginController : Controller
    {
        private Util util = new Util();
        private UsuarioService usuarioService = new UsuarioService();
        private CorreoService correoService = new CorreoService();
        private DenunciaService denunciaService = new DenunciaService();
        private UsuarioRolService usuarioRolService = new UsuarioRolService();
        private Combobox combobox = new Combobox();
        private string identificadorMail = "RecuperaContrasena";
        // GET: Login
        public ActionResult Index()
        {
            Session["SesionActiva"] = null;
            Session["Denuncia"] = null;
            ViewBag.textoLogin = util.TextoLogin;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Validar([Bind(Include = "Usuario,Password")] LoginDto entity)
        {

            SesionUsuarioDto sesionUsuario = new SesionUsuarioDto();
            UsuarioLoginOutputDto usuarioLoginOutputDto = new UsuarioLoginOutputDto();
            vmIntentosFallidos intentos = new vmIntentosFallidos();
            UsuarioBloqueoPassowordInputDto usuarioBloqueo = new UsuarioBloqueoPassowordInputDto();
            List<UsuarioRolListDto> usuarioRolListDtos = new List<UsuarioRolListDto>();
            bool bloqueada = true;  
            try
            {
                if (ModelState.IsValid)
                {
                    
                    string contrasena = util.Encriptar(entity.Password);
                    UsuarioLoginInputDto usuarioLoginInputDto = new UsuarioLoginInputDto()
                    {
                        Rut = entity.Usuario,
                        Password = contrasena
                    };


                    var result = usuarioService.SelectLogin(util.UrlServicios, "0", usuarioLoginInputDto);

                    if (result != null)
                    {
                        if (result.Respuesta.CodigoRetorno == 1)
                        {
                            
                            Session["Intentos"] = null;
                            usuarioLoginOutputDto = JsonConvert.DeserializeObject<UsuarioLoginOutputDto>(result.Resultado.Data.ToString());

                            if (usuarioLoginOutputDto.PasswordBloqueada == 1)
                            {
                                if ((DateTime.Now - Convert.ToDateTime(usuarioLoginOutputDto.FechaBloqueo)).Minutes > Convert.ToInt32(util.TiempoBloqueo))
                                {
                                    bloqueada = false;
                                }
                            }
                            else bloqueada = false;
                            if (!bloqueada)
                            {

                                
                                if (Session["SesionActiva"] == null)
                                {

                                    //Escribiendo cookie para mantener los datos del usuario en pantalla                
                                    HttpContext.Response.Cookies.Add(new HttpCookie("UsuarioId", usuarioLoginOutputDto.IdUsuario.ToString()));
                                    HttpContext.Response.Cookies.Add(new HttpCookie("Usuario", usuarioLoginOutputDto.Usuario.ToString()));
                                    HttpContext.Response.Cookies.Add(new HttpCookie("Entrar", "1"));
                                    HttpContext.Response.Cookies.Add(new HttpCookie("Rol", usuarioLoginOutputDto.IdRol.ToString()));

                                    //Escribiendo Session para mantener los datos del usuario en pantalla
                                    sesionUsuario = new SesionUsuarioDto()
                                    {
                                        UsuarioId = int.Parse(usuarioLoginOutputDto.IdUsuario.ToString()),
                                        Usuario = usuarioLoginOutputDto.Usuario,
                                        Rut = usuarioLoginOutputDto.Rut,
                                        Email = usuarioLoginOutputDto.Email,
                                        //IdRol = usuarioLoginOutputDto.IdRol,
                                        Token = result.Respuesta.Token

                                    };
                                    Session["SesionActiva"] = sesionUsuario;
                                }

                                if (util.FechaToIso(usuarioLoginOutputDto.FechaCambioPassword) != "1900-01-01")
                                {
                                    if ((DateTime.Now - Convert.ToDateTime(usuarioLoginOutputDto.FechaCambioPassword)).Days > Convert.ToInt32(util.TiempoCambioContrasena))
                                    {
                                        return Json(util.RespuestaJson(true, ModelState, "Contraseña caducada, favor modificarla ", "/login/modificaPassword?id=" + usuarioLoginOutputDto.IdUsuario), JsonRequestBehavior.AllowGet);
                                    }
                                }


                                if (Session["Intentos"] == null)
                                {
                                    var resultUsuario = usuarioService.SelectUsuarioByRut(util.UrlServicios, "0", entity.Usuario);
                                    if (resultUsuario != null)
                                    {
                                        if (resultUsuario.Respuesta.CodigoRetorno == 1)
                                        {
                                            intentos.Usuario = entity.Usuario;
                                            intentos.Intentos = 1;
                                            intentos.IdUsuario = JsonConvert.DeserializeObject<UsuarioLoginOutputDto>(resultUsuario.Resultado.Data.ToString()).IdUsuario;
                                            Session["Intentos"] = intentos;
                                        }
                                    }
                                }
                                else
                                {
                                    intentos = (vmIntentosFallidos)Session["Intentos"];
                                    if (intentos.Usuario == entity.Usuario)
                                    {
                                        intentos.Intentos += 1;
                                    }
                                    else
                                    {
                                        Session["Intentos"] = null;
                                        var resultUsuario = usuarioService.SelectUsuarioByRut(util.UrlServicios, "0", entity.Usuario);
                                        if (resultUsuario != null)
                                        {
                                            if (resultUsuario.Respuesta.CodigoRetorno == 1)
                                            {
                                                intentos.Usuario = entity.Usuario;
                                                intentos.Intentos = 1;
                                                intentos.IdUsuario = JsonConvert.DeserializeObject<UsuarioLoginOutputDto>(resultUsuario.Resultado.Data.ToString()).IdUsuario;
                                                Session["Intentos"] = intentos;
                                            }
                                        }
                                    }
                                    if (Session["Intentos"] != null)
                                    {
                                        if (intentos.Intentos.ToString() == util.Intentos)
                                        {
                                            usuarioBloqueo.IdUsuario = Convert.ToInt32(intentos.IdUsuario);
                                            usuarioBloqueo.FechaBloqueo = util.FechaToIsoHour(DateTime.Now.ToString());
                                            var resultBloqueo = usuarioService.UpdateBloqueContrasena(util.UrlServicios, "0", usuarioBloqueo);
                                            if (resultBloqueo != null)
                                            {
                                                if (resultBloqueo.Respuesta.CodigoRetorno == 1)
                                                {
                                                    return Json(util.RespuestaJson(false, ModelState, "Usuario bloqueado por intentos fallidos, intente mas tarde por favor", ""), JsonRequestBehavior.AllowGet);
                                                }
                                            }

                                        }
                                    }

                                }






                                ViewBag.Cantidad = usuarioLoginOutputDto.CantidadRol;
                                ViewBag.Rol = new SelectList(combobox.ComboboxRol("0", usuarioLoginOutputDto.IdUsuario.ToString(), 0), "IdRol", "Nombre", 0);
                                ViewBag.IdUsuario = usuarioLoginOutputDto.IdUsuario;
                                if (usuarioLoginOutputDto.CantidadRol == 1)
                                {
                                    usuarioLoginOutputDto.IdRol = usuarioLoginOutputDto.IdRol;
                                   
                                    sesionUsuario.IdRol = usuarioLoginOutputDto.IdRol;

                                    Session["SesionActiva"] = sesionUsuario;

                                }
                                
                                if (usuarioLoginOutputDto.PasswordTemporal == 0)
                                {
                                    ViewBag.Usuario = sesionUsuario.Usuario;
                                    return PartialView("SeleccionarRol");
                                   // return Json(util.RespuestaJson(true, ModelState, "Bienvenido " + sesionUsuario.Usuario, "/denuncia/index"), JsonRequestBehavior.AllowGet);
                                    
                                }
                                else
                                {
                                    
                                    HttpContext.Response.Cookies.Add(new HttpCookie("Entrar", "2"));
                                    return Json(util.RespuestaJson(true, ModelState, "Bienvenido " + sesionUsuario.Usuario, "/login/modificaPassword?id=" + usuarioLoginOutputDto.IdUsuario), JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                return Json(util.RespuestaJson(false, ModelState, "Usuario bloqueado por intentos fallidos, intente mas tarde por favor", ""), JsonRequestBehavior.AllowGet);
                            }
                        }
                        
                    }

                    
                }
            }
            catch(Exception ex)
            {
                return Json(util.RespuestaJson(false, ModelState, "Problemas para conectar con el servidor "+ex.Message, ""), JsonRequestBehavior.AllowGet);
            }
            return Json(util.RespuestaJson(false, ModelState, "Usuario y/o Password inválidos, intente nuevamente por favor", ""), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarRol([Bind(Include = "idUsuario,idRol")] UsuarioRolInputDto entity)
        {
            SesionUsuarioDto sesionUsuario = new SesionUsuarioDto();
          
                try
            {
                if (ModelState.IsValid)
                {
                    if (Session["SesionActiva"] != null)
                    {
                        sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                        sesionUsuario.IdRol = entity.idRol;
                        Session["SesionActiva"] = sesionUsuario;
                        HttpContext.Response.Cookies.Add(new HttpCookie("Rol",entity.idRol.ToString()));
                    }


                    return Json(util.RespuestaJson(true, ModelState, "Bienvenido " + sesionUsuario.Usuario, "/denuncia/index"), JsonRequestBehavior.AllowGet);
                        
                }

            }
            catch
            {

            }
            return Json(util.RespuestaJson(false, ModelState, "Rol es obligatorio", ""), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Anonimo()
        {
            try
            {
                return Json(util.RespuestaJson(true, ModelState, "Bienvenido, Usuario", "/denuncia/datosinvolucrados"), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult RecuperarPassword()
        {
            return View("RecuperarPassword");
        }



        public ActionResult EnviaMailRecupera(RecuperaContrasenaInputDto entity)
        {
            UsuarioLoginOutputDto usuarioLoginOutputDto = new UsuarioLoginOutputDto();
            RecuperaContrasenaOutputDto recuperaContrasenOutputDto = new RecuperaContrasenaOutputDto();
            CorreoOutputDto correoOutputDto = new CorreoOutputDto();
            try
            {
                if (ModelState.IsValid)
                {
                    var result = usuarioService.SelectUsuarioByRut(util.UrlServicios, "0", entity.Rut);

                    if (result != null)
                    {
                        if (result.Respuesta.CodigoRetorno == 1)
                        {
                            string mailAcotado = "";
                            bool mailEnviado = false;
                            if (result.Respuesta.CodigoRetorno == 1)
                            {
                                usuarioLoginOutputDto = JsonConvert.DeserializeObject<UsuarioLoginOutputDto>(result.Resultado.Data.ToString());

                                string contrasenaTemporal = util.GenerarPassword(6);
                                UsuarioContrasenaTemporalInputDto entityContrasena = new UsuarioContrasenaTemporalInputDto()
                                {
                                    idUsuario = usuarioLoginOutputDto.IdUsuario,
                                    ContrasenaTemporal = util.Encriptar(contrasenaTemporal),
                                    temporal = 1

                                };
                                usuarioService.UpdateContrasenaTemporal(util.UrlServicios, "0", entityContrasena);


                                //recuperaContrasenOutputDto = JsonConvert.DeserializeObject<RecuperaContrasenOutputDto>(result.Resultado.Data.ToString());
                                var resultMail = correoService.CorreoByIdentificadorFuera(util.UrlServicios, "0", identificadorMail);
                                if (resultMail != null)
                                {
                                    if (resultMail.Respuesta.CodigoRetorno == 1)
                                    {
                                        correoOutputDto = JsonConvert.DeserializeObject<CorreoOutputDto>(resultMail.Resultado.Data.ToString());

                                        correoOutputDto.Cuerpo = correoOutputDto.Cuerpo.Replace(correoOutputDto.Parametro.Split('|')[0], usuarioLoginOutputDto.Usuario);//Nombre Usuario
                                        correoOutputDto.Cuerpo = correoOutputDto.Cuerpo.Replace(correoOutputDto.Parametro.Split('|')[1], contrasenaTemporal);//Contraseña Temporal

                                        //envio de mail
                                        mailEnviado = util.SendMail(usuarioLoginOutputDto.Email, correoOutputDto.Asunto, correoOutputDto.Cuerpo);

                                        if (mailEnviado)
                                        {
                                            string[] mail;
                                            mail = usuarioLoginOutputDto.Email.Split('@');
                                            mailAcotado = mail[0].Substring(mail[0].ToString().Length - 2, 2) + "@" + mail[1].ToString();

                                            return Json(util.RespuestaJson(true, ModelState, "Email de recuperación enviado a: xxxxxx " + mailAcotado, "Index"), JsonRequestBehavior.AllowGet);


                                        }
                                        else
                                        {
                                            return Json(util.RespuestaJson(false, ModelState, "Problemas al enviar Email de recuperación, intente nuevamente", ""), JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        return Json(util.RespuestaJson(false, ModelState, "Problemas al enviar Email de recuperación, intente nuevamente", ""), JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    return Json(util.RespuestaJson(false, ModelState, "Problemas al enviar Email de recuperación, intente nuevamente", ""), JsonRequestBehavior.AllowGet);
                                }

                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                return Json(util.RespuestaJson(false, ModelState, "Problemas con el servidor, favor contactar a su administrador "+ex, ""), JsonRequestBehavior.AllowGet);
            }
            return Json(util.RespuestaJson(false, ModelState, "Usuario no encontrado, favor intente nuevamente", ""), JsonRequestBehavior.AllowGet);

        }

        public ActionResult ModificaPassword(int id)
        {
            ViewBag.IdUsuario = id;
            return View("ModificarPassword");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarPassword([Bind(Include = "IdUsuario,ContrasenaNuevaRepeticion,ContrasenaNueva")] UsuarioRecuperaContrasenaInputDto entity)
        {
            SesionUsuarioDto sesionUsuario = new SesionUsuarioDto();
            RecuperaContrasenaOutputDto recuperaContrasenOutputDto = new RecuperaContrasenaOutputDto();
            List<UsuarioPasswordOutputDto> usuarioPasswordOutputDtos = new List<UsuarioPasswordOutputDto>();
            string msj = "";
            try
            {
                
                if (ModelState.IsValid)
                {
                    if (entity.ContrasenaNueva != entity.ContrasenaNuevaRepeticion)
                    {
                        return Json(util.RespuestaJson(false, ModelState, "Las contraseñas no coinciden, intente nuevamente por favor", ""), JsonRequestBehavior.AllowGet);
                    }
                    if(entity.ContrasenaNueva.Length<12 || entity.ContrasenaNueva.Length> 24)
                    {
                        msj = "<p> Entre 12 y 24 caracteres </p>";
                        //return Json(util.RespuestaJson(false, ModelState, "Debe tener al menos 12 caracteres, intente nuevamente por favor", ""), JsonRequestBehavior.AllowGet);
                    }
                     msj += util.ContrasenaSegura(entity.ContrasenaNueva);
                    if(msj != "")
                    {
                        return Json(util.RespuestaJson(false, ModelState, "<p>Debe contener  </p>"+msj, ""), JsonRequestBehavior.AllowGet);
                    }


                    sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                    string contrasenaTemporal = util.Encriptar(entity.ContrasenaNueva);

                    var resultContrasenaAntigua = usuarioService.SelectListContrasena(util.UrlServicios, "0", sesionUsuario.UsuarioId);

                    if (resultContrasenaAntigua != null)
                    {
                        if (resultContrasenaAntigua.Respuesta.CodigoRetorno == 1)
                        {
                            foreach (var item in resultContrasenaAntigua.Resultado.Data)
                            {
                                var resultado = JsonConvert.DeserializeObject<UsuarioPasswordOutputDto>(item.ToString());
                                usuarioPasswordOutputDtos.Add(resultado);
                            }

                            if (usuarioPasswordOutputDtos.Where(x => x.Password == contrasenaTemporal).Count() > 0)
                            {
                                return Json(util.RespuestaJson(false, ModelState, "La contraseña ya fue utilizada anteriormente", ""), JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    UsuarioContrasenaTemporalInputDto entityContrasena = new UsuarioContrasenaTemporalInputDto()
                    {
                        idUsuario = sesionUsuario.UsuarioId,
                        ContrasenaTemporal = util.Encriptar(entity.ContrasenaNueva),
                        temporal = 0,
                        FechaCambio = util.FechaToIso(DateTime.Now.ToString())

                    };
                    usuarioService.UpdateContrasenaTemporal(util.UrlServicios, "0", entityContrasena);

                    return Json(util.RespuestaJson(true, ModelState, "Su contraseña ha sido modificada", "Index"), JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {

            }
            return Json(util.RespuestaJson(false, ModelState, "Ha ocurrido un problema, intente nuevamente por favor", ""), JsonRequestBehavior.AllowGet);

        }

        public ActionResult IndexAnonimo()
        {
            return View("IndexAnonimo");
        }

        public ActionResult BuscarDenuncia(string Denuncia)
        {
            SesionUsuarioDto sesionUsuario = new SesionUsuarioDto();
            int idDenuncia = 0;
            try
            {
                var result = usuarioService.SelectAnonimo(util.UrlServicios);
                if (result != null)
                {
                    var resultado = denunciaService.SelectIdDenunciaByDenuncia(util.UrlServicios, result.Respuesta.Token, Denuncia);
                    if (resultado != null)
                    {
                        if (resultado.Respuesta.CodigoRetorno == 1)
                        {

                            idDenuncia = JsonConvert.DeserializeObject<DenunciaIdOutputDto>(resultado.Resultado.Data.ToString()).IdDenuncia;

                            if (Session["SesionActiva"] == null)
                            {
                                //Escribiendo cookie para mantener los datos del usuario en pantalla                
                                HttpContext.Response.Cookies.Add(new HttpCookie("UsuarioId", "1"));
                                HttpContext.Response.Cookies.Add(new HttpCookie("Usuario", "Anónimo"));
                                HttpContext.Response.Cookies.Add(new HttpCookie("Rut", "0-0"));
                                HttpContext.Response.Cookies.Add(new HttpCookie("Matriz", "N/A"));
                                HttpContext.Response.Cookies.Add(new HttpCookie("Sucursal", "N/A"));
                                HttpContext.Response.Cookies.Add(new HttpCookie("Email", "N/A"));
                                HttpContext.Response.Cookies.Add(new HttpCookie("Entrar", "1"));

                                //Escribiendo Session para mantener los datos del usuario en pantalla
                                sesionUsuario = new SesionUsuarioDto()
                                {
                                    UsuarioId = 1,
                                    Usuario = "Anónimo",
                                    Rut = "0-0",
                                    Matriz = "N/A",
                                    Sucursal = "N/A",
                                    Email = "N/A",
                                    IdRol = 3,
                                    Token = result.Respuesta.Token
                                };
                                Session["SesionActiva"] = sesionUsuario;
                            }
                        }
                        else
                        {
                            return Json(util.RespuestaJson(false, ModelState, "No se encontraron datos asociados a la denuncia", ""), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(util.RespuestaJson(false, ModelState, "No se encontraron datos asociados a la denuncia", ""), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(util.RespuestaJson(false, ModelState, "No se encontraron datos asociados a la denuncia", ""), JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {

            }
            return Json(util.RespuestaJson(true, ModelState, "Denuncia encontrada con éxito", "/denuncia/DatosInvolucrados?idDenuncia=" + idDenuncia + "&desde=1"), JsonRequestBehavior.AllowGet);
            //return RedirectToAction("DatosInvolucrados","Denuncia", new { @idDenuncia = idDenuncia, @desde = 1 });
        }

        public ActionResult NuevaDenunciaAnonimo()
        {
            SesionUsuarioDto sesionUsuario = new SesionUsuarioDto();
            int idDenuncia = 0;
            try
            {
                var result = usuarioService.SelectAnonimo(util.UrlServicios);

                if (result != null)
                {
                    if (result.Respuesta.CodigoRetorno == 1)
                    {

                        if (Session["SesionActiva"] == null)
                        {
                            //Escribiendo cookie para mantener los datos del usuario en pantalla                
                            HttpContext.Response.Cookies.Add(new HttpCookie("UsuarioId", "1"));
                            HttpContext.Response.Cookies.Add(new HttpCookie("Usuario", "ANONIMO"));
                            HttpContext.Response.Cookies.Add(new HttpCookie("Rut", "0-0"));
                            HttpContext.Response.Cookies.Add(new HttpCookie("Matriz", "N/A"));
                            HttpContext.Response.Cookies.Add(new HttpCookie("Sucursal", "N/A"));
                            HttpContext.Response.Cookies.Add(new HttpCookie("Email", "N/A"));
                            HttpContext.Response.Cookies.Add(new HttpCookie("Entrar", "1"));
                            HttpContext.Response.Cookies.Add(new HttpCookie("Rol", "3"));

                            //Escribiendo Session para mantener los datos del usuario en pantalla
                            sesionUsuario = new SesionUsuarioDto()
                            {
                                UsuarioId = 1,
                                Usuario = "ANONIMO",
                                Rut = "0-0",
                                Matriz = "N/A",
                                Sucursal = "N/A",
                                Email = "N/A",
                                IdRol = 3,
                                Token = result.Respuesta.Token


                            };
                            Session["SesionActiva"] = sesionUsuario;
                        }
                    }
                }

            }
            catch
            {

            }
            return PartialView("CorreoAnonimo");
        }

       

        public ActionResult GuardarCorreo(string Email)
        {
            int idDenuncia = 0;
            SesionUsuarioDto sesionUsuario = new SesionUsuarioDto();
            if (Email != "")
            {
                if (Session["SesionActiva"] != null)
                {
                    sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                    sesionUsuario.CorreoAnonimo = Email;
                }
            }
            return Json(util.RespuestaJson(true, ModelState, "Datos guardados con éxito", "/denuncia/DatosInvolucrados?idDenuncia=" + idDenuncia + "&desde=1"), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Close()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}
