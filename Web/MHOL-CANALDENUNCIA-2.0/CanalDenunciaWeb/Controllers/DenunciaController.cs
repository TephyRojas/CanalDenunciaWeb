using CanalDenunciaWeb.Attribute;
using CanalDenunciaWeb.Data.Models;
using CanalDenunciaWeb.Data.Package.CorreoPkg;
using CanalDenunciaWeb.Data.Package.DenunciaComentarioPkg;
using CanalDenunciaWeb.Data.Package.DenunciaCorreoAnonimoPkg;
using CanalDenunciaWeb.Data.Package.DenunciaDocumentoHistoricoPkg;
using CanalDenunciaWeb.Data.Package.DenunciaDocumentoPkg;
using CanalDenunciaWeb.Data.Package.DenunciaHistoricoPkg;
using CanalDenunciaWeb.Data.Package.DenunciaInvolucradoHistoricoPkg;
using CanalDenunciaWeb.Data.Package.DenunciaInvolucradoPkg;
using CanalDenunciaWeb.Data.Package.DenunciaOficialCumplimientoPkg;
using CanalDenunciaWeb.Data.Package.DenunciaPkg;
using CanalDenunciaWeb.Data.Package.DepartamentoSedePkg;
using CanalDenunciaWeb.Data.Package.TipoDelitoPkg;
using CanalDenunciaWeb.Data.Package.UsuarioPkg;
using CanalDenunciaWeb.Data.Package.UsuarioRolPkg;
using CanalDenunciaWeb.Helper;
using CanalDenunciaWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CanalDenunciaWeb.Controllers
{
    [LoginAuthenticate]
    public class DenunciaController : Controller
    {
        private UsuarioService usuarioService = new UsuarioService();
        private Util util = new Util();
        private EnvioMail envioMail = new EnvioMail();
        private SesionUsuarioDto sesionUsuario = new SesionUsuarioDto();
        private Combobox combobox = new Combobox();
        private DenunciaService denunciaService = new DenunciaService();
        private DenunciaDocumentoHistoricoService denunciaDocumentoHistoricoService = new DenunciaDocumentoHistoricoService();
        private DepartamentoSedeService departamentoSedeService = new DepartamentoSedeService();
        private DenunciaInvolucradoService denunciaInvolucradoService = new DenunciaInvolucradoService();
        private TipoDelitoService tipoDelitoService = new TipoDelitoService();
        private DenunciaDocumentoService denunciaDocumentoService = new DenunciaDocumentoService();
        private DenunciaComentarioService denunciaComentarioService = new DenunciaComentarioService();
        private CorreoService correoService = new CorreoService();
        private DenunciaHistoricoService denunciaHistoricoService = new DenunciaHistoricoService();
        private DenunciaInvolucradoHistoricoService denunciaInvolucradoHistoricoService = new DenunciaInvolucradoHistoricoService();
        private UsuarioRolService usuarioRolService = new UsuarioRolService();
        private DenunciaCorreoAnonimoService denunciaCorreoAnonimoService = new DenunciaCorreoAnonimoService();
        private DenunciaOficialCumplimientoService denunciaOficialCumplimientoService = new DenunciaOficialCumplimientoService();
        private static string rutaArchivos = "~/Archivos/D_";

        // GET: Denuncia
        [HttpGet]
        public ActionResult Index()
        {
            List<DenunciaListOutputDto> denunciaListOutputDtos = new List<DenunciaListOutputDto>();
            string idUsuario = "";
            try
            {

                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];

                //if (sesionUsuario.IdRol.ToString() != "1")
                //{
                    idUsuario = sesionUsuario.UsuarioId.ToString();
                //}
                

                var result = denunciaService.SelectList(util.UrlServicios, sesionUsuario.Token, "", "", "", "", "", "", "", "", "", idUsuario, sesionUsuario.IdRol.ToString());

                if (result != null)
                {
                    if (result.Respuesta.CodigoRetorno == 1)
                    {
                        sesionUsuario.Token = result.Respuesta.Token;
                        if (result.Resultado.Data.Count > 0)
                        {
                            foreach (var item in result.Resultado.Data)
                            {
                                var resultDenuncia = JsonConvert.DeserializeObject<DenunciaListOutputDto>(item.ToString());
                                denunciaListOutputDtos.Add(resultDenuncia);
                            }
                        }
                    }
                }

                /* Combobox  */
                ViewBag.Perfil = sesionUsuario.IdRol.ToString();
                ViewBag.Denuncia = "";
                ViewBag.FechaIngreso = "";
                ViewBag.FechaOcurrencia = "";
                ViewBag.Sede = new SelectList(combobox.ComboboxSede(sesionUsuario.Token), "IdSede", "Descripcion", 0);
                ViewBag.TipoDelito = new SelectList(combobox.ComboboxTipoDelito(sesionUsuario.Token), "IdTipoDelito", "Nombre", 0);
                ViewBag.Estado = new SelectList(combobox.ComboboxEstado(sesionUsuario.Token), "IdEstadoDenuncia", "Nombre", 0);
                ViewBag.IdSede = 0;
                ViewBag.IdEstado = 0;
                ViewBag.IdTipoDelito = 0;
                ViewBag.IdUsuario = sesionUsuario.UsuarioId;
                ViewBag.RolDenunciante = util.RolDenunciante;
                ViewBag.RolComite = util.RolComiteEtica;
                ViewBag.RolOficial = util.RolOficialCumplimiento;

            }
            catch (Exception ex)
            {
                return Json(util.RespuestaJson(false, ModelState, ex.Message, ""), JsonRequestBehavior.AllowGet);
            }
            return View("Index", denunciaListOutputDtos);
        }
        // GET: Denuncia/IndexFiltro
        [HttpGet]
        public ActionResult IndexFiltro(string Denuncia, string FechaDenuncia, string FechaOcurrencia, string IdSede, string IdDepartamento, string IdTipoDelito, string IdEstado)
        {
            List<DenunciaListOutputDto> denunciaListOutputDtos = new List<DenunciaListOutputDto>();
            string idUsuario = "";
            try
            {
                /*======================================== Session y Perfilamiento ========================================*/
                //if (CheckPermiso() == 0)
                //{
                //    return RedirectToAction("Error403", "Error");
                //}
                /*===========================================================================================================*/

                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                string inicioDenuncia = "", finDenuncia = "", inicioOcurrencia = "", finOcurrencia = "";
                if (FechaDenuncia != null)
                {
                    if (FechaDenuncia.Length > 0)
                    {
                        string[] fechas = FechaDenuncia.Split('-');
                        inicioDenuncia = util.FechaToIso(fechas[0].ToString().Trim());
                        finDenuncia = util.FechaToIso(fechas[1].ToString().Trim());
                    }
                }

                if (FechaOcurrencia != null)
                {
                    if (FechaOcurrencia.Length > 0)
                    {
                        string[] fechas = FechaOcurrencia.Split('-');
                        inicioOcurrencia = util.FechaToIso(fechas[0].ToString().Trim());
                        finOcurrencia = util.FechaToIso(fechas[1].ToString().Trim());
                    }
                }

                if (sesionUsuario.IdRol.ToString() == "3")
                {
                    idUsuario = sesionUsuario.IdRol.ToString();
                }

                var result = denunciaService.SelectList(util.UrlServicios, sesionUsuario.Token, Denuncia, inicioDenuncia, finDenuncia, inicioOcurrencia, finOcurrencia, IdSede, IdDepartamento, IdTipoDelito, IdEstado, idUsuario);

                if (result != null)
                {
                    if (result.Respuesta.CodigoRetorno == 1)
                    {
                        sesionUsuario.Token = result.Respuesta.Token;
                        if (result.Resultado.Data.Count > 0)
                        {
                            foreach (var item in result.Resultado.Data)
                            {
                                var resultDenuncia = JsonConvert.DeserializeObject<DenunciaListOutputDto>(item.ToString());
                                denunciaListOutputDtos.Add(resultDenuncia);
                            }
                        }
                    }
                }


                /*
              * Combobox
              */
                ViewBag.Sede = new SelectList(combobox.ComboboxSede(sesionUsuario.Token), "IdSede", "Descripcion", IdSede);
                if (IdSede.Length > 0)
                {
                    if (IdSede != "0")
                    {
                        ViewBag.Departamento = new SelectList(combobox.ComboboxDepartamento(sesionUsuario.Token, 1, Convert.ToInt32(IdSede)), "IdDepartamento", "Nombre", IdDepartamento);
                    }
                }

                ViewBag.Denuncia = Denuncia;
                ViewBag.FechaIngreso = FechaDenuncia;
                ViewBag.FechaOcurrencia = FechaOcurrencia;
                ViewBag.TipoDelito = new SelectList(combobox.ComboboxTipoDelito(sesionUsuario.Token), "IdTipoDelito", "Nombre", IdTipoDelito);
                ViewBag.Estado = new SelectList(combobox.ComboboxEstado(sesionUsuario.Token), "IdEstadoDenuncia", "Nombre", IdEstado);
                ViewBag.IdSede = IdSede;
                ViewBag.IdTipoDelito = IdTipoDelito;
                ViewBag.IdEstado = IdEstado;
                ViewBag.IdUsuario = sesionUsuario.UsuarioId;
                ViewBag.Error = 0;
                ViewBag.Perfil = sesionUsuario.IdRol.ToString();
                ViewBag.RolDenunciante = util.RolDenunciante;
                ViewBag.Comite = util.RolComiteEtica;
                ViewBag.Oficial = util.RolOficialCumplimiento;

                return View("Index", denunciaListOutputDtos);

            }
            catch
            {
                //util.logSistema(ex.Message.ToString() + "|" + ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return RedirectToAction("Error500", "Error");
            }

        }

        [HttpGet]
        public ActionResult DatosPersonales(int id)
        {
            UsuarioLoginOutputDto usuarioLoginOutputDto = new UsuarioLoginOutputDto();
            Session["Denuncia"] = null;
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                var result = usuarioService.SelectUsuarioById(util.UrlServicios, sesionUsuario.Token, id);
                if (result != null)
                {
                    if (result.Respuesta.CodigoRetorno == 1)
                    {
                        sesionUsuario.Token = result.Respuesta.Token;
                        usuarioLoginOutputDto = JsonConvert.DeserializeObject<UsuarioLoginOutputDto>(result.Resultado.Data.ToString());
                        ViewBag.Rut = usuarioLoginOutputDto.Rut;
                        ViewBag.Nombre = usuarioLoginOutputDto.Usuario;
                        ViewBag.Telefono = usuarioLoginOutputDto.Telefono;
                        ViewBag.Email = usuarioLoginOutputDto.Email;
                        ViewBag.IdUsuario = id;
                    }
                }
            }
            catch
            {

            }
            return PartialView("DenunciaDatosPersonales");
        }
        //POST: Denuncia/ModificarDatosPersonales

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarDatosPersonales([Bind(Include ="IdUsuario,Telefono,Email")]UsuarioDatosPersonalesInputDto entity)
        {
            UsuarioDatosPersonalesInputDto usuarioDatosPersonalesInputDto = new UsuarioDatosPersonalesInputDto();
            try
            {
                if (ModelState.IsValid)
                {
                    sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                    ViewBag.IdDenuncia = 0;
                    usuarioDatosPersonalesInputDto = entity;
                    if(entity.Telefono == null)
                    {
                        entity.Telefono = "0";
                    }
                    var result = usuarioService.UpdateDatosPersonales(util.UrlServicios, sesionUsuario.Token, usuarioDatosPersonalesInputDto);
                    if (result != null)
                    {
                        if (result.Respuesta.CodigoRetorno == 1)
                        {
                            sesionUsuario.Token = result.Respuesta.Token;
                            return Json(util.RespuestaJson(true, ModelState, "Datos Modificados con éxito", "/denuncia/datosInvolucrados?idDenuncia=0"), JsonRequestBehavior.AllowGet);
                        }
                    }
                }

            }
            catch
            {

            }
            return Json(util.RespuestaJson(false, ModelState, "Email es obligatorio", ""), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetDepartamentoByIdSede(string id, int opcionSeleccione = 1)
        {
            List<DepartamentoSedeComboboxOutputDto> departamentoSedeComboboxOutputDtos = new List<DepartamentoSedeComboboxOutputDto>();

            try
            {
                /*======================================== Session y Perfilamiento ========================================*/
                //if (CheckPermiso() == 0)
                //{
                //    return RedirectToAction("Error403", "Error");
                //}
                /*===========================================================================================================*/
                /*
                 *  Consulta los departamentos de la sede especificada
                 */
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                var resultDepartamentoSede = departamentoSedeService.SelectListCombobox(util.UrlServicios, sesionUsuario.Token, id);

                if (resultDepartamentoSede.Respuesta.CodigoRetorno == 1)
                {
                    sesionUsuario.Token = resultDepartamentoSede.Respuesta.Token;
                    if (resultDepartamentoSede.Resultado.Data.Count > 0)
                    {
                        foreach (var item in resultDepartamentoSede.Resultado.Data)
                        {
                            var result = JsonConvert.DeserializeObject<DepartamentoSedeComboboxOutputDto>(item.ToString());
                            departamentoSedeComboboxOutputDtos.Add(result);
                        }

                        if (opcionSeleccione == 1)
                        {
                            DepartamentoSedeComboboxOutputDto seleccione = new DepartamentoSedeComboboxOutputDto()
                            {
                                IdDepartamentoSede = 0,
                                Nombre = " - Seleccione - ",
                            };

                            departamentoSedeComboboxOutputDtos.Add(seleccione);
                        }

                        departamentoSedeComboboxOutputDtos = departamentoSedeComboboxOutputDtos.OrderBy(x => x.Nombre).ToList();

                    }
                    return Json(departamentoSedeComboboxOutputDtos, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(util.RespuestaJson(false, ModelState, "Ha ocurrido un error, Intente nuevamente. Si el error persiste, comuníquese con el administrador.", ""), JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                //util.logSistema(ex.Message.ToString() + "|" + ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return RedirectToAction("Error500", "Error");
            }
        }
        [HttpGet]
        public ActionResult Cancelar()
        {
            sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
            Session["Denuncia"] = null;
            if (sesionUsuario.UsuarioId != 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("IndexAnonimo", "Login");
            }
        }

        #region Involucrados
        [HttpGet]
        public ActionResult DatosInvolucrados(int idDenuncia, int desde = 0)
        {
            SesionDenunciaDto sesionDenuncia = new SesionDenunciaDto();
            vmDenunciaInvolucradoDto vmDenunciaInvolucradoDto = new vmDenunciaInvolucradoDto();
            List<DenunciaInvolucradoListOutputDto> denunciaInvolucradoListOutputDto = new List<DenunciaInvolucradoListOutputDto>();
            List<DenunciaInvolucradoListOutputDto> denunciaInvolucradoListOutputDtos = new List<DenunciaInvolucradoListOutputDto>();
            //DenunciaInvolucradoListOutputDto denunciaInvolucradoListOutput = new DenunciaInvolucradoListOutputDto();
            //List<DenunciaDetalleDto> denunciaDetalleDtos = new List<DenunciaDetalleDto>();
            DenunciaDetalleDto denunciaDetalleDto = new DenunciaDetalleDto();
            Denuncia denuncia = new Denuncia();
            List<Involucrados> listInvolucrados = new List<Involucrados>();
            List<Comentarios> listComentario = new List<Comentarios>();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                if (desde == 1)
                {
                    Session["Denuncia"] = null;
                }
                ViewBag.Perfil = sesionUsuario.IdRol;
                ViewBag.RolOficial = Convert.ToInt32(util.RolOficialCumplimiento);
                ViewBag.RolSuperOficial = Convert.ToInt32(util.RolSuperOficial);
                ViewBag.VerEstado = ValidaCambioEstado( idDenuncia);
                if (sesionUsuario.IdRol.ToString() == util.RolDenunciante)
                {
                    ViewBag.Modificar = 0;
                }
                else 
                {
                    ViewBag.Modificar = 1;
                }
                ViewBag.IdDenuncia = idDenuncia;

                if (Session["Denuncia"] != null)
                {
                    denuncia = (Denuncia)Session["Denuncia"];

                    if (denuncia.involucrados != null)
                    {
                        foreach (var involucrado in denuncia.involucrados)
                        {
                            if (involucrado.tipo == 1)
                            {
                                DenunciaInvolucradoListOutputDto denunciaInvolucrado = new DenunciaInvolucradoListOutputDto
                                {
                                    IdDenuncia = denuncia.idDenuncia,
                                    IdUsuario = involucrado.idUsuario,
                                    Nombre = involucrado.nombre,
                                    Sede = involucrado.sede,
                                    Departamento = involucrado.departamento
                                };
                                denunciaInvolucradoListOutputDto.Add(denunciaInvolucrado);
                            }
                            else
                            {
                                DenunciaInvolucradoListOutputDto denunciaInvolucrado = new DenunciaInvolucradoListOutputDto
                                {
                                    IdDenuncia = denuncia.idDenuncia,
                                    IdUsuario = involucrado.idUsuario,
                                    Nombre = involucrado.nombre,
                                    Sede = involucrado.sede,
                                    Departamento = involucrado.departamento
                                };
                                denunciaInvolucradoListOutputDtos.Add(denunciaInvolucrado);
                            }
                        }
                    }
                }
                else
                {
                    if (idDenuncia != 0)
                    {

                        var resultDenuncia = denunciaService.SelectDenuncia(util.UrlServicios, sesionUsuario.Token, idDenuncia);

                        if (resultDenuncia != null)
                        {
                            if (resultDenuncia.Respuesta.CodigoRetorno == 1)
                            {
                                sesionUsuario.Token = resultDenuncia.Respuesta.Token;
                                denunciaDetalleDto = JsonConvert.DeserializeObject<DenunciaDetalleDto>(resultDenuncia.Resultado.Data.ToString());
                                //Lenar la sesion denuncia

                                denuncia.idDenuncia = idDenuncia;
                                denuncia.Identificador = denunciaDetalleDto.Denuncia;
                                denuncia.PermiteModificacion = denunciaDetalleDto.PermiteModificacion;
                                denuncia.IdEstadoDenuncia = denunciaDetalleDto.IdEstadoDenuncia;
                                Detalle detalle = new Detalle
                                {
                                    Comentario = denunciaDetalleDto.Descripcion,
                                    FechaFin = denunciaDetalleDto.FechaFin,
                                    FechaInicio = denunciaDetalleDto.FechaInicio,
                                    IdTipoDelito = denunciaDetalleDto.IdTipoDelito,
                                    IdDepartamentoSede = denunciaDetalleDto.IdDepartamentoSede,
                                    IdSede = denunciaDetalleDto.IdSede,
                                    Descripcion = denunciaDetalleDto.Descripcion
                                };
                                denuncia.detalles = detalle;
                                if (denuncia.PermiteModificacion == 0)
                                {
                                    ViewBag.Modificar = 1;
                                }
                            }
                        }

                        if (denunciaDetalleDto.Involucrados != null)
                        {
                            if (denunciaDetalleDto.Involucrados.Count > 0)
                            {
                                foreach (var involucrados in denunciaDetalleDto.Involucrados)
                                {
                                    Involucrados denunciado = new Involucrados();
                                    denunciado.idUsuario = involucrados.IdUsuario;
                                    denunciado.nombre = involucrados.Nombre;
                                    denunciado.sede = involucrados.Sede;
                                    denunciado.departamento = involucrados.Departamento;
                                    if (involucrados.IdRol.ToString() == util.RolDenunciante)
                                    {
                                        denunciado.tipo = 1;
                                        DenunciaInvolucradoListOutputDto denunciaInvolucrado = new DenunciaInvolucradoListOutputDto
                                        {
                                            IdDenuncia = denuncia.idDenuncia,
                                            IdUsuario = involucrados.IdUsuario,
                                            Nombre = involucrados.Nombre,
                                            Sede = involucrados.Sede,
                                            Departamento = involucrados.Departamento
                                        };
                                        denunciaInvolucradoListOutputDto.Add(denunciaInvolucrado);
                                    }
                                    else
                                    {
                                        denunciado.tipo = 0;
                                        DenunciaInvolucradoListOutputDto denunciaInvolucrado = new DenunciaInvolucradoListOutputDto
                                        {
                                            IdDenuncia = denuncia.idDenuncia,
                                            IdUsuario = involucrados.IdUsuario,
                                            Nombre = involucrados.Nombre,
                                            Sede = involucrados.Sede,
                                            Departamento = involucrados.Departamento
                                        };
                                        denunciaInvolucradoListOutputDtos.Add(denunciaInvolucrado);
                                    }
                                    listInvolucrados.Add(denunciado);

                                }
                            }

                        }

                        if (denunciaDetalleDto.Comentarios != null)
                        {
                            if (denunciaDetalleDto.Comentarios.Count > 0)
                            {
                                foreach (var comentario in denunciaDetalleDto.Comentarios)
                                {
                                    Comentarios comentarios = new Comentarios()
                                    {
                                        IdDenunciaComentario = comentario.IdDenunciaComentario,
                                        Usuario = comentario.Usuario,
                                        Comentario = comentario.Comentario,
                                        Fecha = comentario.Fecha
                                    };
                                    listComentario.Add(comentarios);
                                }
                            }
                        }

                        denuncia.involucrados = listInvolucrados;
                        denuncia.comentarios = listComentario;
                        Session["Denuncia"] = denuncia;
                    }
                    else
                    {
                        denuncia.PermiteModificacion = 1;
                        denuncia.idDenuncia = idDenuncia;
                        denuncia.involucrados = listInvolucrados;
                        denuncia.directorio = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                        Session["Denuncia"] = denuncia;

                    }
                }

                vmDenunciaInvolucradoDto.listInvolucrado = denunciaInvolucradoListOutputDto;
                vmDenunciaInvolucradoDto.listInvolucradoCE = denunciaInvolucradoListOutputDtos;



                //var result = denunciaInvolucradoService.SelectList(util.UrlServicios,"1",idDenuncia)
            }
            catch (Exception ex)
            {
                return Json(util.RespuestaJson(false, ModelState, ex.Message, ""), JsonRequestBehavior.AllowGet);
            }
            return View(vmDenunciaInvolucradoDto);
        }
        //[HttpGet]

        //public ActionResult InvolucradoDenunciante(string idDenuncia)
        //{
        //    List<DenunciaInvolucradoListOutputDto> denunciaInvolucradoListOutputDtos = new List<DenunciaInvolucradoListOutputDto>();
        //    Denuncia denuncia = new Denuncia();
        //    try
        //    {
        //        sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
        //        if (Session["Denuncia"] != null)
        //        {
        //            denuncia = (Denuncia)Session["Denuncia"];
        //        }

        //        var result = denunciaInvolucradoService.SelectListByIdRol(util.UrlServicios, sesionUsuario.Token, util.RolDenunciante, "", "0", "0");

        //        if (result != null)
        //        {
        //            if (result.Respuesta.CodigoRetorno == 1)
        //            {
        //                sesionUsuario.Token = result.Respuesta.Token;
        //                if (result.Resultado.Data.Count > 0)
        //                {
        //                    foreach (var item in result.Resultado.Data)
        //                    {
        //                        var resultDenunciaInvolucrado = JsonConvert.DeserializeObject<DenunciaInvolucradoListOutputDto>(item.ToString());
        //                        if (denuncia.involucrados != null)
        //                        {
        //                            if ((denuncia.involucrados.Where(x => x.idUsuario == resultDenunciaInvolucrado.IdUsuario && x.tipo == 1)).Count() == 0)
        //                            {
        //                                denunciaInvolucradoListOutputDtos.Add(resultDenunciaInvolucrado);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            denunciaInvolucradoListOutputDtos.Add(resultDenunciaInvolucrado);
        //                        }
        //                    }
        //                }
        //            }
        //        }


        //        /* Combobox  */
        //        ViewBag.Perfil = sesionUsuario.IdRol;
        //        ViewBag.Denuncia = idDenuncia;
        //        ViewBag.Nombre = "";
        //        ViewBag.Sede = new SelectList(combobox.ComboboxSede(sesionUsuario.Token), "IdSede", "Descripcion");


        //        ViewBag.IdSede = "0";
        //        ViewBag.IdDepartamento = "0";
        //        ViewBag.IdUsuario = sesionUsuario.UsuarioId;
        //        ViewBag.Involucrado = denunciaInvolucradoListOutputDtos;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(util.RespuestaJson(false, ModelState, ex.Message, ""), JsonRequestBehavior.AllowGet);

        //    }
        //    return View(denunciaInvolucradoListOutputDtos);

        //}
        [HttpGet]
        public ActionResult InvolucradosDenunciante(string idDenuncia, string idSede ="0", string idDepartamento ="", string nombre = "")
        {
            List<DenunciaInvolucradoListOutputDto> denunciaInvolucradoListOutputDtos = new List<DenunciaInvolucradoListOutputDto>();
            Denuncia denuncia = new Denuncia();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                if (Session["Denuncia"] != null)
                {
                    denuncia = (Denuncia)Session["Denuncia"];
                }
                if (idSede == null)
                {
                    idSede = "0";
                }
                if (nombre == null)
                {
                    nombre = "";
                }
                if (idDepartamento == null)
                {
                    idDepartamento = "0";
                }
                var result = denunciaInvolucradoService.SelectListByIdRol(util.UrlServicios, sesionUsuario.Token, util.RolDenunciante, nombre, idSede, idDepartamento,util.RolComiteEtica+","+util.RolOficialCumplimiento+","+util.RolSuperOficial); ;

                if (result != null)
                {
                    if (result.Respuesta.CodigoRetorno == 1)
                    {
                        sesionUsuario.Token = result.Respuesta.Token;
                        if (result.Resultado.Data.Count > 0)
                        {
                            foreach (var item in result.Resultado.Data)
                            {
                                var resultDenunciaInvolucrado = JsonConvert.DeserializeObject<DenunciaInvolucradoListOutputDto>(item.ToString());
                                if (denuncia.involucrados != null)
                                {
                                    if ((denuncia.involucrados.Where(x => x.idUsuario == resultDenunciaInvolucrado.IdUsuario && x.tipo == 1)).Count() == 0)
                                    {
                                        denunciaInvolucradoListOutputDtos.Add(resultDenunciaInvolucrado);
                                    }
                                }
                                else
                                {
                                    denunciaInvolucradoListOutputDtos.Add(resultDenunciaInvolucrado);
                                }
                            }
                        }
                    }
                }


                /* Combobox  */
                ViewBag.Perfil = sesionUsuario.IdRol;
                ViewBag.Denuncia = idDenuncia;
                ViewBag.Nombre = nombre;
                ViewBag.Sede = new SelectList(combobox.ComboboxSede(sesionUsuario.Token), "IdSede", "Descripcion", idSede);
                if (idDepartamento != "0")
                {
                    ViewBag.Departamento = new SelectList(combobox.ComboboxDepartamento(sesionUsuario.Token), "idDepartamento", "Nombre", idDepartamento);
                }
                else
                {
                    ViewBag.Departamento = null;
                }
                ViewBag.IdSede = idSede;
                ViewBag.IdDepartamento = idDepartamento;
                ViewBag.IdUsuario = sesionUsuario.UsuarioId;
                ViewBag.Involucrado = denunciaInvolucradoListOutputDtos;
            }
            catch 
            {
                return View("Error");
            }
            return View("InvolucradoDenunciante");

        }
        [HttpGet]
        public ActionResult InvolucradosCE(int idDenuncia, string nombre = "", int idSede = 0, int idDepartamento = 0)
        {

            List<DenunciaInvolucradoListOutputDto> denunciaInvolucradoListOutputDtos = new List<DenunciaInvolucradoListOutputDto>();
            Denuncia denuncia = new Denuncia();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                if (Session["Denuncia"] != null)
                {
                    denuncia = (Denuncia)Session["Denuncia"];
                }

                var result = denunciaInvolucradoService.SelectListByIdRol(util.UrlServicios, sesionUsuario.Token, util.RolComiteEtica + "," + util.RolOficialCumplimiento+","+util.RolSuperOficial, nombre, idSede.ToString(), idDepartamento.ToString()); ;

                if (result != null)
                {
                    if (result.Respuesta.CodigoRetorno == 1)
                    {
                        sesionUsuario.Token = result.Respuesta.Token;
                        if (result.Resultado.Data.Count > 0)
                        {
                            foreach (var item in result.Resultado.Data)
                            {
                                var resultDenunciaInvolucrado = JsonConvert.DeserializeObject<DenunciaInvolucradoListOutputDto>(item.ToString());
                                if (denuncia.involucrados != null)
                                {
                                    if ((denuncia.involucrados.Where(x => x.idUsuario == resultDenunciaInvolucrado.IdUsuario && x.tipo == 0)).Count() == 0)
                                    {
                                        denunciaInvolucradoListOutputDtos.Add(resultDenunciaInvolucrado);
                                    }
                                }
                                else
                                {
                                    denunciaInvolucradoListOutputDtos.Add(resultDenunciaInvolucrado);
                                }
                            }
                        }
                    }
                }


                /* Combobox  */
                ViewBag.Perfil = sesionUsuario.IdRol;
                ViewBag.Denuncia = idDenuncia;
                ViewBag.Nombre = nombre;
                ViewBag.Sede = new SelectList(combobox.ComboboxSede(sesionUsuario.Token), "IdSede", "Descripcion", idSede);
                if (idDepartamento > 0)
                {
                    ViewBag.Departamento = new SelectList(combobox.ComboboxDepartamento(sesionUsuario.Token), "idDepartamento", "Nombre", idDepartamento);
                }
                else
                {
                    ViewBag.Departamento = null;
                }
                ViewBag.IdSede = idSede;
                ViewBag.IdDepartamento = idDepartamento;
                ViewBag.IdUsuario = sesionUsuario.UsuarioId;
                ViewBag.Involucrado = denunciaInvolucradoListOutputDtos;
            }
            catch
            {

            }
            return View("InvolucradosCE");

        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarInvolucrados([Bind(Include = "IdDenuncia,Involucrados")] vmInvolucradoInputDto entity)  
        {
            List<Involucrados> involucrados = new List<Involucrados>();
            List<int> usuarios = new List<int>();
            Denuncia denuncia = new Denuncia();
            UsuarioOutputDto usuarioOutputDto = new UsuarioOutputDto();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                if (entity.Involucrados == null)
                {
                    return Json(util.RespuestaJson(false, ModelState, "Debe seleccionar al menos un Involucrado", ""), JsonRequestBehavior.AllowGet);
                }

                if (ModelState.IsValid)
                {
                    if (Session["Denuncia"] != null)
                    {
                        denuncia = (Denuncia)Session["Denuncia"];
                        involucrados = denuncia.involucrados;

                    }
                    else
                    {
                        denuncia.idDenuncia = entity.IdDenuncia;
                    }

                    foreach (var involucrado in entity.Involucrados)
                    {
                        var busqueda = involucrados.Count(x => x.idUsuario == involucrado.IdUsuario);
                        if (busqueda == 0)
                        {
                            var result = usuarioService.SelectUsuarioByIdUsuario(util.UrlServicios, sesionUsuario.Token, involucrado.IdUsuario);
                            if (result != null)
                            {
                                if (result.Respuesta.CodigoRetorno == 1)
                                {
                                    usuarioOutputDto = JsonConvert.DeserializeObject<UsuarioOutputDto>(result.Resultado.Data.ToString());
                                }
                            }
                            Involucrados listado = new Involucrados()
                            {
                                idUsuario = involucrado.IdUsuario,
                                nombre = usuarioOutputDto.Nombre,
                                sede = usuarioOutputDto.Sede,
                                departamento = usuarioOutputDto.Departamento,
                                tipo = 1

                            };
                            involucrados.Add(listado);
                        }
                    }

                    denuncia.involucrados = involucrados;
                    Session["Denuncia"] = denuncia;

                    return Json(util.RespuestaJson(true, ModelState, "Datos guardados con éxito", "/denuncia/datosInvolucrados?idDenuncia=" + entity.IdDenuncia), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(util.RespuestaJson(false, ModelState, "Debes completar todos los campos requeridos, Intente nuevamente.", ""), JsonRequestBehavior.AllowGet);
                }
            }
            catch 
            {
                //util.logSistema(ex.Message.ToString() + "|" + ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return RedirectToAction("Error500", "Error");
            }
        }

        // POST: Involucrados/GuardarComité
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarInvolucradosCE([Bind(Include = "IdDenuncia,Involucrados")] vmInvolucradoInputDto entity)
        {
            List<Involucrados> involucrados = new List<Involucrados>();
            List<int> usuarios = new List<int>();
            Denuncia denuncia = new Denuncia();
            UsuarioOutputDto usuarioOutputDto = new UsuarioOutputDto();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                if (entity.Involucrados == null)
                {
                    return Json(util.RespuestaJson(false, ModelState, "Debe seleccionar al menos un Involucrado", ""), JsonRequestBehavior.AllowGet);
                }

                if (ModelState.IsValid)
                {
                    if (Session["Denuncia"] != null)
                    {
                        denuncia = (Denuncia)Session["Denuncia"];
                        involucrados = denuncia.involucrados;

                    }
                    else
                    {
                        denuncia.idDenuncia = entity.IdDenuncia;
                    }

                    foreach (var involucrado in entity.Involucrados)
                    {
                        var busqueda = involucrados.Count(x => x.idUsuario == involucrado.IdUsuario);
                        if (busqueda == 0)
                        {
                            var result = usuarioService.SelectUsuarioByIdUsuario(util.UrlServicios, sesionUsuario.Token, involucrado.IdUsuario);
                            if (result != null)
                            {
                                if (result.Respuesta.CodigoRetorno == 1)
                                {
                                    sesionUsuario.Token = result.Respuesta.Token;
                                    usuarioOutputDto = JsonConvert.DeserializeObject<UsuarioOutputDto>(result.Resultado.Data.ToString());
                                }
                            }
                            Involucrados listado = new Involucrados()
                            {
                                idUsuario = involucrado.IdUsuario,
                                nombre = usuarioOutputDto.Nombre,
                                sede = usuarioOutputDto.Sede,
                                departamento = usuarioOutputDto.Departamento,
                                tipo = 0

                            };
                            involucrados.Add(listado);
                        }
                    }

                    denuncia.involucrados = involucrados;

                    //Validar que no todos los usuarios estén involucrados
                    List<UsuarioRolListDto> usuarioRolList = new List<UsuarioRolListDto>();
                    var resultado = usuarioRolService.SelectUsuarioRolByIdRol(util.UrlServicios, sesionUsuario.Token, util.RolComiteEtica + "," + util.RolOficialCumplimiento);
                    if (resultado != null)
                    {
                        if (resultado.Respuesta.CodigoRetorno == 1)
                        {
                            foreach (var item in resultado.Resultado.Data)
                            {
                                var result = JsonConvert.DeserializeObject<UsuarioRolListDto>(item.ToString());
                                usuarioRolList.Add(result);
                            }
                        }
                    }

                    List<Involucrados> involucradosCE = new List<Involucrados>();
                    involucradosCE = denuncia.involucrados;

                    foreach (var item in involucradosCE)
                    {
                        usuarioRolList.RemoveAll(x => x.IdUsuario == item.idUsuario);
                    }

                    if (usuarioRolList.Count() == 0)
                    {//TO DO: Cambiar texto
                        return Json(util.RespuestaJson(true, ModelState, "No podemos dar seguimiento en Chile a una denuncia que involucre a oficial de cumplimiento y el comité de ética.", "/denuncia/NoRegistra"), JsonRequestBehavior.AllowGet);
                    }

                    Session["Denuncia"] = denuncia;

                    return Json(util.RespuestaJson(true, ModelState, "Datos guardados con éxito", "/denuncia/datosInvolucrados?idDenuncia=" + entity.IdDenuncia), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(util.RespuestaJson(false, ModelState, "Debes completar todos los campos requeridos, Intente nuevamente.", ""), JsonRequestBehavior.AllowGet);
                }
            }
            catch 
            {
                //util.logSistema(ex.Message.ToString() + "|" + ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return RedirectToAction("Error500", "Error");
            }
        }

        // GET: Involucrado/Delete/5
        [HttpGet]
        public ActionResult Delete(int id, int idDenuncia)
        {
            Denuncia denuncia = new Denuncia();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                if (Session["Denuncia"] != null)
                {
                    denuncia = (Denuncia)Session["Denuncia"];
                }
                denuncia.involucrados.Remove(denuncia.involucrados.First(x => x.idUsuario == id));
                Session["Denuncia"] = denuncia;
                return Json(util.RespuestaJson(true, ModelState, "Datos eliminados correctamente", "/denuncia/datosInvolucrados?idDenuncia=" + idDenuncia), JsonRequestBehavior.AllowGet);

            }
            catch 
            {
                //util.logSistema(ex.Message.ToString() + "|" + ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                return RedirectToAction("Error500", "Error");
            }
        }
        #endregion
        #region Adjuntos

        private vmArchivo GetFiles(int idDenuncia)
        {
            Denuncia denuncia = new Denuncia();
            vmArchivo vmArchivo = new vmArchivo();
            denuncia = (Denuncia)Session["Denuncia"];
            List<string> items = new List<string>();
            long peso = 0;
            string ruta = "";
            if (idDenuncia != 0)
            {
                ruta = Server.MapPath(rutaArchivos + idDenuncia);
            }
            else
            {
                ruta = Server.MapPath(rutaArchivos + denuncia.directorio);
            }

            if (Directory.Exists(ruta))
            {
                var dir = new System.IO.DirectoryInfo(ruta);
                System.IO.FileInfo[] fileNames = dir.GetFiles("*.*");


                foreach (var file in fileNames)
                {
                    items.Add(file.Name);
                    peso += file.Length;
                }

                
            }
            vmArchivo.nombreArchivo = items;
            vmArchivo.pesoArchivo = peso;
            return vmArchivo;
        }
        [HttpGet]
        public ActionResult DatosArchivosAdjuntos(int idDenuncia)
        {
            SesionDenunciaDto sesionDenuncia = new SesionDenunciaDto();
            List<string> archivos = new List<string>();
            Denuncia denuncia = new Denuncia();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                ViewBag.RolSuperOficial = Convert.ToInt32(util.RolSuperOficial);
                ViewBag.VerEstado = ValidaCambioEstado(idDenuncia);
                if (sesionUsuario.IdRol.ToString() == util.RolDenunciante)
                {
                    ViewBag.Modificar = 0;
                }
                else
                {
                    ViewBag.Modificar = 1;
                }
                if (Session["Denuncia"] != null)
                {
                    denuncia = (Denuncia)Session["Denuncia"];
                    if (denuncia.PermiteModificacion == 0)
                    {
                        ViewBag.Modificar = 1;
                    }

                }
                else
                {
                    denuncia.idDenuncia = idDenuncia;
                    Session["Denuncia"] = denuncia;
                }
                List<string> nombresArchivos = new List<string>();
                //long peso = 0;
                //if (denuncia.documentos != null)
                //{
                //    foreach (var file in denuncia.documentos)
                //    {
                //        nombresArchivos.Add(file.nombre);
                //    }
                //    peso = denuncia.documentos.Sum(x => x.archivo.ContentLength);
                //}
                //else
                //{
                //    peso = 0;
                //}

                vmArchivo vmArchivo = new vmArchivo();
                vmArchivo = GetFiles(idDenuncia);


                archivos = vmArchivo.nombreArchivo;
                ViewBag.Peso = vmArchivo.pesoArchivo / (1024 * 1024);
                //archivos = nombresArchivos;
                //ViewBag.Peso = peso / (1024 * 1024);
                ViewBag.Cantidad = archivos==null?0:archivos.Count;
                if (ViewBag.Peso == 0)
                {
                    if (ViewBag.Cantidad > 0)
                        ViewBag.Peso = 1;
                }
                ViewBag.IdDenuncia = idDenuncia;
                ViewBag.Perfil = sesionUsuario.IdRol;
                ViewBag.RolOficial = Convert.ToInt32(util.RolOficialCumplimiento);



            }
            catch 
            {
                return RedirectToAction("Error500", "Error");
            }
            return View(archivos);
        }

        [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult GuardarArchivosAdjuntos(HttpPostedFileBase file)
        {
            Denuncia denuncia = new Denuncia();
            List<Documentos> listDocumentos = new List<Documentos>();
            List<string> archivos = new List<string>();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];

                if (sesionUsuario.IdRol.ToString() == util.RolDenunciante)
                {
                    ViewBag.Modificar = 0;
                }
                else
                {
                    ViewBag.Modificar = 1;
                };
                ViewBag.Perfil = sesionUsuario.IdRol;

                if (Session["Denuncia"] != null)
                {
                    denuncia = (Denuncia)Session["Denuncia"];
                    ViewBag.IdDenuncia = denuncia.idDenuncia;
                    if (denuncia.documentos != null)
                    {
                        listDocumentos = denuncia.documentos;
                    }
                    if (denuncia.PermiteModificacion == 0)
                    {
                        ViewBag.Modificar = 1;
                    }
                }
                vmArchivo vmArchivo = new vmArchivo();
                vmArchivo = GetFiles(denuncia.idDenuncia);
                archivos = vmArchivo.nombreArchivo;
                if (archivos.Count >= 5)
                {
                    return Json(util.RespuestaJson(false, ModelState, "Ha ocurrido un error, Intente nuevamente. Si el error persiste, comuníquese con el administrador.", ""), JsonRequestBehavior.AllowGet);
                }
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        long pesoTotal = (vmArchivo.pesoArchivo + file.ContentLength) / (1024 * 1024);

                        if (pesoTotal <= 20)
                        {
                            
                            string ruta = "";
                            if (denuncia.idDenuncia != 0)
                            {
                                ruta = Server.MapPath(rutaArchivos + denuncia.idDenuncia);

                            }
                            else
                            {
                                ruta = Server.MapPath(rutaArchivos + denuncia.directorio);
                            }
                            // Si el directorio no existe, crearlo
                            if (!Directory.Exists(ruta))
                                Directory.CreateDirectory(ruta);

                            string archivo = String.Format("{0}\\{1}", ruta, file.FileName);


                            if (!System.IO.File.Exists(archivo))
                            {
                                file.SaveAs(archivo);
                                Documentos documentos = new Documentos()
                                {
                                    archivo = file,
                                    nombre = file.FileName,
                                    extension = System.IO.Path.GetExtension(file.FileName)
                                };
                                listDocumentos.Add(documentos);
                            }
                            
                        }
                    }
                }
                denuncia.documentos = listDocumentos;
                Session["Denuncia"] = denuncia;
            }
            catch
            {

            }
            return RedirectToAction("DatosArchivosAdjuntos", new { idDenuncia = ViewBag.IdDenuncia });
        }

        public FileResult Descargar(string Archivo, string IdDenuncia)
        {
            string ruta = "";
            string[] regreso = new string[] { "" };
            Denuncia denuncia = new Denuncia();
            if (Session["Denuncia"] != null)
            {
                denuncia = (Denuncia)Session["Denuncia"];
            }
           
            if (IdDenuncia != "0")
            {
                ruta = Server.MapPath(rutaArchivos + IdDenuncia);
            }
            else
            {
                ruta = Server.MapPath(rutaArchivos + denuncia.directorio);
            }


           
            var FileVirtualPath = ruta + "/" + Archivo;
            if (util.Extensiones.Contains(Path.GetExtension(FileVirtualPath)))
            {
                return File(FileVirtualPath, "application/force- download", Path.GetFileName(FileVirtualPath));
            }

            return null;
        }
        
       
        public ActionResult EliminarDocumento(string Archivo, string IdDenuncia)
        {
            Denuncia denuncia = new Denuncia();
            string path = "";
            if (Session["Denuncia"] != null)
            {
                denuncia = (Denuncia)Session["Denuncia"];
            }
            string [] extensiones = util.Extensiones.Split(',');
            string [] extensionArchivo = Archivo.Split('.');

            if (extensiones.Contains("."+extensionArchivo[1]))
            {

                if (IdDenuncia != "0")
                {
                    path = Server.MapPath(rutaArchivos + IdDenuncia);
                }
                else
                {
                    path = Server.MapPath(rutaArchivos + denuncia.directorio);
                }

                string fullpath = Path.Combine(path, Archivo);
                if (util.Extensiones.Contains(Path.GetExtension(Archivo)))
                {
                    System.IO.File.Delete(fullpath);
                }


                if (denuncia.documentos != null)
                {
                    if (denuncia.documentos.Count > 0)
                    {
                        denuncia.documentos.Remove(denuncia.documentos.Where(x => x.nombre == Archivo).FirstOrDefault());
                    }
                }
            }
            return RedirectToAction("DatosArchivosAdjuntos", new { idDenuncia = IdDenuncia });

        }

        #endregion
        #region Detalles
        [HttpGet]
        public ActionResult DatosDetalles(int idDenuncia)
        {

            List<Comentarios> lstComentarios = new List<Comentarios>();
            Denuncia denuncia = new Denuncia();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                ViewBag.Modificar = 0;
                ViewBag.Perfil = sesionUsuario.IdRol;
                ViewBag.RolOficial = Convert.ToInt32(util.RolOficialCumplimiento);
                ViewBag.RolSuperOficial = Convert.ToInt32(util.RolSuperOficial);
                ViewBag.VerEstado = ValidaCambioEstado(idDenuncia);
                ViewBag.IdDenuncia = idDenuncia;
                if (sesionUsuario.IdRol.ToString() == util.RolDenunciante)
                {
                    ViewBag.Modificar = 0;
                }
                else
                {
                    ViewBag.Modificar = 1;
                }

                if (Session["Denuncia"] != null)
                {
                    denuncia = (Denuncia)Session["Denuncia"];
                    if (denuncia.comentarios == null)
                    {
                        denuncia.comentarios = lstComentarios;
                    }
                    if (denuncia.PermiteModificacion == 0)
                    {
                        ViewBag.Modificar = 1;
                    }
                }


                if (denuncia.detalles != null)
                {
                    ViewBag.Delito = new SelectList(combobox.ComboboxTipoDelito(sesionUsuario.Token), "IdTipoDelito", "Nombre", denuncia.detalles.IdTipoDelito);
                    ViewBag.Sede = new SelectList(combobox.ComboboxSede(sesionUsuario.Token), "IdSede", "Descripcion", denuncia.detalles.IdSede);
                    ViewBag.Departamento = new SelectList(combobox.ComboboxDepartamentoSede(sesionUsuario.Token, 1, denuncia.detalles.IdSede), "IdDepartamentoSede", "Nombre", denuncia.detalles.IdDepartamentoSede);
                    ViewBag.Descripcion = denuncia.detalles.Comentario;
                    ViewBag.Fecha = denuncia.detalles.FechaInicio + " - " + denuncia.detalles.FechaFin;
                    ViewBag.Descripcion = denuncia.detalles.Descripcion;
                }
                else
                {
                    ViewBag.Delito = new SelectList(combobox.ComboboxTipoDelito(sesionUsuario.Token), "IdTipoDelito", "Nombre", 0);
                    ViewBag.Sede = new SelectList(combobox.ComboboxSede(sesionUsuario.Token), "IdSede", "Descripcion", 0);
                }


            }
            catch 
            {
                return RedirectToAction("Error500", "Error");
            }
            return View(denuncia.comentarios);
        }

        // Textarea Delito por id Json       
        public ActionResult GetDetalleByIdTipoDelito(string idTipoDelito)
        {
            TipoDelitoDescripcionOutputDto tipoDelitoDescripcionOutputDto = new TipoDelitoDescripcionOutputDto();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                /*
                 *  Consulta las los datos del tipo de delito
                 */
                var resultDelito = tipoDelitoService.SelectDescripcionByIdTipoDelito(util.UrlServicios, sesionUsuario.Token, idTipoDelito);
                if (resultDelito != null)
                {
                    if (resultDelito.Respuesta.CodigoRetorno == 1)
                    {
                        sesionUsuario.Token = resultDelito.Respuesta.Token;
                        var result = JsonConvert.DeserializeObject<TipoDelitoDescripcionOutputDto>(resultDelito.Resultado.Data.ToString());
                        tipoDelitoDescripcionOutputDto = result;
                    }
                }
                else
                {
                    return Json(util.RespuestaJson(false, ModelState, "Ha ocurrido un error, Intente nuevamente. Si el error persiste, comuníquese con el administrador.", ""), JsonRequestBehavior.AllowGet);
                }
                return Json(tipoDelitoDescripcionOutputDto, JsonRequestBehavior.AllowGet);
            }
            catch 
            {
                return RedirectToAction("Error500", "Error");
            }
        }


        // POST: Involucrados/Guardar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarDetalles([Bind(Include = "IdDenuncia,Fecha,IdTipoDelito,Descripcion,IdDepartamentoSede")]vmDenunciaDetalle entity)
        {
            List<UsuarioEmailOutputDto> usuarioOficialOutputDtos = new List<UsuarioEmailOutputDto>();
            List<UsuarioRolListDto> usuarioBackupListDtos = new List<UsuarioRolListDto>();
            Detalle detalle = new Detalle();
            Denuncia denuncia = new Denuncia();
            //DenunciaInputDto denunciaInputDto = new DenunciaInputDto();
            UsuarioOutputDto usuarioOutputDto = new UsuarioOutputDto();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                if (ModelState.IsValid)
                {
                    if (Session["Denuncia"] != null)
                    {
                        denuncia = (Denuncia)Session["Denuncia"];
                    }
                    else
                    {
                        denuncia.idDenuncia = entity.IdDenuncia;
                    }

                    //Identificador de denuncia

                    string inicioOcurrencia = "";
                    string finOcurrencia = "";
                    if (entity.Fecha != null)
                    {
                        if (entity.Fecha.Length > 0)
                        {
                            string[] fechas = entity.Fecha.Split('-');
                            inicioOcurrencia = util.FechaToIso(fechas[0].ToString().Trim());
                            finOcurrencia = util.FechaToIso(fechas[1].ToString().Trim());
                        }
                    }
                    detalle.IdTipoDelito = entity.IdTipoDelito;
                    detalle.FechaInicio = inicioOcurrencia;
                    detalle.FechaFin = finOcurrencia;

                    //detalle.Comentario = entity.Descripcion;

                    denuncia.detalles = detalle;
                    Session["Denuncia"] = denuncia;

                    //const string alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    //StringBuilder token = new StringBuilder();
                    //Random rnd = new Random();
                    //int indice = rnd.Next(alfabeto.Length);

                    string indice = util.GetRandomSeed().ToString().Substring(1, 3);
                    if (denuncia.idDenuncia == 0)
                    {
                        denuncia.Identificador = "MARU-" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + indice.ToString();
                    }
                    //*******************Guardar en bd***********************//
                    //**Guardar Denuncia - Cabecera ***
                    bool exito = true;
                    int idGenerado = denuncia.idDenuncia;
                    DenunciaInputDto denunciaInput = new DenunciaInputDto()
                    {
                        IdTipoDelito = denuncia.detalles.IdTipoDelito,
                        IdEstadoDenuncia = 1,//Valor de inicio
                        FechaIngreso = util.FechaToIso(DateTime.Now.ToString()),
                        FechaInicio = denuncia.detalles.FechaInicio,
                        FechaFin = denuncia.detalles.FechaFin,
                        //Descripcion = denuncia.detalles.Comentario,
                        Denuncia = denuncia.Identificador,
                        IdUsuario = sesionUsuario.UsuarioId,
                        IdDepartamentoSede = entity.IdDepartamentoSede,
                        Activo = 1
                    };
                    RespuestaOutputDto resultDenuncia = new RespuestaOutputDto();
                    if (denuncia.idDenuncia == 0)
                    {
                        resultDenuncia = denunciaService.Insert(util.UrlServicios, sesionUsuario.Token, denunciaInput);
                    }
                    else
                    {
                        denunciaInput.IdDenuncia = denuncia.idDenuncia;
                        var resultSelectDenuncia = denunciaService.SelectDenuncia(util.UrlServicios, sesionUsuario.Token, denuncia.idDenuncia);
                        if (resultSelectDenuncia != null)
                        {
                            if (resultSelectDenuncia.Respuesta.CodigoRetorno == 1)
                            {
                                var resultadoDenuncia = JsonConvert.DeserializeObject<DenunciaOutputDto>(resultSelectDenuncia.Resultado.Data.ToString());

                                DenunciaHistoricoInputDto data = new DenunciaHistoricoInputDto()
                                {
                                    IdDenuncia = denuncia.idDenuncia,
                                    IdDepartamentoSede = resultadoDenuncia.IdDepartamentoSede,
                                    IdEstadoDenuncia = resultadoDenuncia.IdEstadoDenuncia,
                                    IdTipoDelito = resultadoDenuncia.IdTipoDelito,
                                    IdUsuarioModificacion = sesionUsuario.UsuarioId,
                                    FechaInicio = resultadoDenuncia.FechaInicio,
                                    FechaFin = resultadoDenuncia.FechaFin,
                                    FechaModificacion = util.FechaToIso(DateTime.Now.ToString()),

                                };
                                var resultHistorico = denunciaHistoricoService.Insert(util.UrlServicios, sesionUsuario.Token, data);
                            }
                        }


                        resultDenuncia = denunciaService.Update(util.UrlServicios, sesionUsuario.Token, denunciaInput);
                    }

                    if (resultDenuncia.Respuesta.CodigoRetorno == 1)
                    {
                        if (denuncia.idDenuncia == 0)
                        {
                            idGenerado = resultDenuncia.Resultado.Data;
                        }
                        if (entity.Descripcion != null && entity.Descripcion != "")
                        {
                            DenunciaComentarioInputDto denunciaComentarioInputDto = new DenunciaComentarioInputDto()
                            {
                                IdDenuncia = idGenerado,
                                IdUsuario = sesionUsuario.UsuarioId,
                                Comentario = entity.Descripcion,
                                Fecha = util.FechaToIso(DateTime.Now.ToString()),
                                Activo = 1
                            };

                            var resultComentario = denunciaComentarioService.Insert(util.UrlServicios, sesionUsuario.Token, denunciaComentarioInputDto);

                            if (resultComentario != null)
                            {
                                if (resultComentario.Respuesta.CodigoRetorno != 1)
                                {
                                    exito = false;
                                }
                            }
                        }

                        if (denuncia.idDenuncia != 0)
                        {
                            List<DenunciaInvolucradoOutputDto> denunciaInvolucradoOutputDtos = new List<DenunciaInvolucradoOutputDto>();
                            var resultListInvolucrado = denunciaInvolucradoService.SelectByIdDenuncia(util.UrlServicios, sesionUsuario.Token, idGenerado.ToString());
                            if (resultListInvolucrado != null)
                            {
                                if (resultListInvolucrado.Respuesta.CodigoRetorno == 1)
                                {
                                    foreach (var item in resultListInvolucrado.Resultado.Data)
                                    {
                                        var result = JsonConvert.DeserializeObject<DenunciaInvolucradoOutputDto>(item.ToString());
                                        denunciaInvolucradoOutputDtos.Add(result);
                                    }
                                }
                                foreach (var item in denunciaInvolucradoOutputDtos)
                                {
                                    DenunciaInvolucradoHistoricoInputDto data = new DenunciaInvolucradoHistoricoInputDto()
                                    {
                                        idDenuncia = item.idDenuncia,
                                        IdDenunciaInvolucrado = item.IdDenunciaInvolucrado,
                                        idUsuario = item.idUsuario,
                                        FechaModificacion = util.FechaToIso(DateTime.Now.ToString())
                                    };
                                    var resultInsertHistorico = denunciaInvolucradoHistoricoService.Insert(util.UrlServicios, sesionUsuario.Token, data);
                                }

                                denunciaInvolucradoService.DeleteByIdDenuncia(util.UrlServicios, sesionUsuario.Token, idGenerado);
                            }
                        }



                        var usuarioOficial = correoService.EmailByIdRol(util.UrlServicios, sesionUsuario.Token, util.RolOficialCumplimiento);
                        if (usuarioOficial != null)
                        {
                            if (usuarioOficial.Respuesta.CodigoRetorno == 1)
                            {
                                foreach (var item in usuarioOficial.Resultado.Data)
                                {
                                    var resultadoUsuarioOficial = JsonConvert.DeserializeObject<UsuarioEmailOutputDto>(item.ToString());
                                    usuarioOficialOutputDtos.Add(resultadoUsuarioOficial);
                                }
                            }
                        }


                        //if (usuarioOficialOutputDtos.Count() <= 0)
                        //{
                            var resultBackUp = usuarioRolService.SelectUsuarioRolByBackup(util.UrlServicios, sesionUsuario.Token);
                            if (resultBackUp != null)
                            {
                                if (resultBackUp.Respuesta.CodigoRetorno == 1)
                                {
                                    foreach (var item in resultBackUp.Resultado.Data)
                                    {
                                        var resultadoBackUp = JsonConvert.DeserializeObject<UsuarioRolListDto>(item.ToString());
                                        usuarioBackupListDtos.Add(resultadoBackUp);
                                    }
                                }
                            }
                        //}

                        if (denuncia.involucrados != null)
                        {

                            //**Guardar Denuncia - DenunciaInvolucrado ***
                            foreach (var involucrado in denuncia.involucrados)
                            {
                                DenunciaInvolucradoInputDto denunciaInvolucrado = new DenunciaInvolucradoInputDto()
                                {
                                    idDenuncia = idGenerado,
                                    idUsuario = involucrado.idUsuario,
                                    Activo = 1
                                };

                                var resultDenunciaInvolucrado = denunciaInvolucradoService.Insert(util.UrlServicios, sesionUsuario.Token, denunciaInvolucrado);
                                if (resultDenunciaInvolucrado != null)
                                {
                                    exito = true;
                                    if (resultDenunciaInvolucrado.Respuesta.CodigoRetorno != 1)
                                    {
                                        exito = false;
                                    }
                                }

                                usuarioOficialOutputDtos.Remove(usuarioOficialOutputDtos.Where(x => x.IdUsuario == involucrado.idUsuario).FirstOrDefault());
                                usuarioBackupListDtos.Remove(usuarioBackupListDtos.Where(x => x.IdUsuario == involucrado.idUsuario).FirstOrDefault());
                            }


                        }
                        else
                        {
                            exito = false;
                        }
                        if (exito)
                        {
                            if (denuncia.idDenuncia != 0)
                            {
                                List<DenunciaDocumentoOutputDto> denunciaDocumentoOutputDtos = new List<DenunciaDocumentoOutputDto>();
                                var resultListDenunciaDocumento = denunciaDocumentoService.SelectByIdDenuncia(util.UrlServicios, sesionUsuario.Token, idGenerado.ToString());
                                if (resultListDenunciaDocumento != null)
                                {
                                    if (resultListDenunciaDocumento.Respuesta.CodigoRetorno == 1)
                                    {
                                        foreach (var item in resultListDenunciaDocumento.Resultado.Data)
                                        {
                                            var result = JsonConvert.DeserializeObject<DenunciaDocumentoOutputDto>(item.ToString());
                                            denunciaDocumentoOutputDtos.Add(result);
                                        }
                                    }
                                    foreach (var item in denunciaDocumentoOutputDtos)
                                    {
                                        DenunciaDocumentoHistoricoInputDto data = new DenunciaDocumentoHistoricoInputDto()
                                        {
                                            IdDenuncia = item.IdDenuncia,
                                            IdDenunciaDocumento = item.IdDenunciaDocumento,
                                            Nombre = item.Nombre,
                                            Extension = item.Extension,
                                            Ruta = item.Ruta,
                                            FechaModificacion = util.FechaToIso(DateTime.Now.ToString())
                                        };
                                        var resultInsertHistorico = denunciaDocumentoHistoricoService.Insert(util.UrlServicios, sesionUsuario.Token, data);
                                    }

                                    denunciaDocumentoService.DeleteByIdDenuncia(util.UrlServicios, sesionUsuario.Token, idGenerado);
                                }
                            }
                            if (denuncia.documentos != null)
                            {
                                foreach (var documento in denuncia.documentos)
                                {
                                    DenunciaDocumentoInputDto denunciaDocumento = new DenunciaDocumentoInputDto()
                                    {
                                        IdDenuncia = idGenerado,
                                        Nombre = documento.nombre,
                                        Extension = documento.extension,
                                        Ruta = rutaArchivos + idGenerado,
                                        Activo = 1
                                    };

                                    var resultDenunciaDocumento = denunciaDocumentoService.Insert(util.UrlServicios, sesionUsuario.Token, denunciaDocumento);

                                    if (resultDenunciaDocumento != null)
                                    {

                                        if (resultDenunciaDocumento.Respuesta.CodigoRetorno != 1)
                                        {
                                            exito = false;
                                        }
                                    }

                                }

                                if (exito)
                                {
                                    if (denuncia.idDenuncia == 0)
                                    {
                                        foreach (var doc in denuncia.documentos)
                                        {
                                            try
                                            {
                                                HttpPostedFileBase file = doc.archivo;

                                                string path = Server.MapPath(rutaArchivos + denuncia.directorio);
                                                string fullpath = Path.Combine(path, file.FileName);
                                                if (System.IO.File.Exists(fullpath))
                                                {
                                                    System.IO.File.Delete(fullpath);

                                                }


                                                string ruta = Server.MapPath(rutaArchivos + idGenerado);

                                                // Si el directorio no existe, crearlo                                       
                                                if (!Directory.Exists(ruta))
                                                    Directory.CreateDirectory(ruta);
                                                string archivo = String.Format("{0}\\{1}", ruta, file.FileName);
                                                if (!System.IO.File.Exists(archivo))
                                                {
                                                    file.SaveAs(archivo);

                                                }
                                            }
                                            catch
                                            {
                                                throw;
                                            }

                                        }

                                        string rutaFinal = Server.MapPath(rutaArchivos + denuncia.directorio);
                                        if (System.IO.Directory.Exists(rutaFinal))
                                        {
                                            System.IO.Directory.Delete(rutaFinal);
                                        }
                                    }

                                }

                            }
                            if (exito)
                            {
                                if (sesionUsuario.UsuarioId == 1)
                                {
                                    if (sesionUsuario.CorreoAnonimo != "")
                                    {
                                        DenunciaCorreoAnonimoInputDto denunciaCorreoAnonimoInputDto = new DenunciaCorreoAnonimoInputDto();
                                        denunciaCorreoAnonimoInputDto.IdDenuncia = idGenerado;
                                        denunciaCorreoAnonimoInputDto.Email = sesionUsuario.CorreoAnonimo;

                                        var result = denunciaCorreoAnonimoService.Insert(util.UrlServicios, sesionUsuario.Token, denunciaCorreoAnonimoInputDto);


                                    }

                                }


                            }
                        }


                        else
                        {
                            return Json(util.RespuestaJson(false, ModelState, "Debes completar todos los campos requeridos, Intente nuevamente.", ""), JsonRequestBehavior.AllowGet);
                        }
                        if (!exito)
                        {
                            denunciaDocumentoService.DeleteByIdDenuncia(util.UrlServicios, sesionUsuario.Token, resultDenuncia.Resultado.Data);
                            denunciaInvolucradoService.DeleteByIdDenuncia(util.UrlServicios, sesionUsuario.Token, resultDenuncia.Resultado.Data);
                            denunciaService.Delete(util.UrlServicios, sesionUsuario.Token, resultDenuncia.Resultado.Data);
                            return Json(util.RespuestaJson(false, ModelState, "Ocurrió un error inesperado, Intente nuevamente.", ""), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Envio de correo
                            if (denuncia.idDenuncia == 0)
                            {
                                if (sesionUsuario.UsuarioId != 1)//usuario logueado
                                {
                                    EnvioCorreo("IngresoDenunciante");
                                }
                                else
                                {
                                    if (sesionUsuario.CorreoAnonimo != "")//usuario anonimo con correo
                                    {
                                        EnvioCorreo("IngresoDenuncianteAnonimo");
                                    }
                                }
                                if (usuarioOficialOutputDtos.Count() > 0)//si hay algun oficial que no esté involcurado en la denuncia
                                {
                                    EnvioCorreo("IngresoOficial", usuarioOficialOutputDtos);
                                }
                                else
                                {
                                    if (usuarioBackupListDtos.Count > 0)//si todos los oficiales de cumplimiento están involucrados o no hay activos pero hay backup
                                    {
                                        EnvioCorreo("IngresoOficialBackup");
                                    }
                                    else //si todos los oficiales y bakcup están involucrados pero no los comité de ética
                                    {
                                        DenunciaOficialCumplimientoInputDto denunciaOficial = new DenunciaOficialCumplimientoInputDto()
                                        {
                                            IdDenuncia = idGenerado,
                                            Activo = 1
                                        };
                                        var result = denunciaOficialCumplimientoService.Insert(util.UrlServicios, sesionUsuario.Token, denunciaOficial); 
                                        EnvioCorreo("IngresoSuperOficial");
                                    }
                                }
                            }
                            else
                            {

                                if (usuarioOficialOutputDtos.Count() > 0)//si hay algun oficial que no esté involcurado en la denuncia
                                {
                                    EnvioCorreo("NuevosAntecedentesOficial");
                                }
                                else
                                {
                                    if (usuarioBackupListDtos.Count > 0)//si todos los oficiales de cumplimiento están involucrados o no hay activos pero hay backup
                                    {
                                        EnvioCorreo("NuevosAntecedentesBackup");
                                    }
                                    else //si todos los oficiales y bakcup están involucrados pero no los comité de ética
                                    {
                                        
                                        EnvioCorreo("NuevosAntecedentesSuperOficial");
                                    }
                                }
                            }
                           
                        }
                    }
                    else
                    {
                        return Json(util.RespuestaJson(false, ModelState, "Debes completar todos los campos requeridos, Intente nuevamente.", ""), JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(util.RespuestaJson(false, ModelState, ex.Message, ""), JsonRequestBehavior.AllowGet);
                //util.logSistema(ex.Message.ToString() + "|" + ex.StackTrace.ToString(), 0, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", "", System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                //return RedirectToAction("Error500", "Error");
            }
            if (denuncia.idDenuncia == 0)
            {
                return Json(util.RespuestaJson(true, ModelState, "Datos guardados con éxito", "/denuncia/Resumen?idDenuncia=" + entity.IdDenuncia), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(util.RespuestaJson(true, ModelState, "Datos modificados con éxito", "/denuncia/Index"), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Resumen()
        {
            Denuncia denuncia = new Denuncia();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                denuncia = (Denuncia)Session["Denuncia"];
            }
            catch
            {

            }
            return View(denuncia);
        }

        public ActionResult ResumenDenuncia()
        {
            Denuncia denuncia = new Denuncia();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                denuncia = (Denuncia)Session["Denuncia"];
            }
            catch
            {

            }
            return View(denuncia);
        }

        public ActionResult NoRegistra()
        {
            Denuncia denuncia = new Denuncia();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                denuncia = (Denuncia)Session["Denuncia"];
            }
            catch
            {

            }
            
            return View(denuncia);
        }
        [HttpGet]
        public ActionResult MostrarObservacion(int id)
        {
            UsuarioLoginOutputDto usuarioLoginOutputDto = new UsuarioLoginOutputDto();
            Denuncia denuncia = new Denuncia();
            DenunciaComentarioOutputDto resultComentario = new DenunciaComentarioOutputDto();
            try
            {

                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                if (Session["Denuncia"] != null)
                {
                    denuncia = (Denuncia)Session["Denuncia"];
                }
                var result = denunciaComentarioService.SelectById(util.UrlServicios, sesionUsuario.Token, id);
                if (result != null)
                {
                    if (result.Respuesta.CodigoRetorno == 1)
                    {
                        resultComentario = JsonConvert.DeserializeObject<DenunciaComentarioOutputDto>(result.Resultado.Data.ToString());

                    }
                }
            }
            catch
            {

            }
            return PartialView("vmObservacion", resultComentario);
        }

        #endregion
        #region Estado
        public ActionResult DatosEstado(int idDenuncia)
        {
            List<Comentarios> lstComentarios = new List<Comentarios>();
            Denuncia denuncia = new Denuncia();
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
               
                if ((Denuncia)Session["Denuncia"] != null)
                {
                    denuncia = (Denuncia)Session["Denuncia"];
                    if (denuncia.comentarios == null)
                    {
                        denuncia.comentarios = lstComentarios;
                    }
                }
                ViewBag.IdDenuncia = idDenuncia;
                ViewBag.Estado = new SelectList(combobox.ComboboxEstado(sesionUsuario.Token), "IdEstadoDenuncia", "Nombre", denuncia.IdEstadoDenuncia);
            }
            catch
            {

            }
            return View(denuncia.comentarios);
        }
        public ActionResult GuardarEstado(vmDenunciaEstado entity)
        {
            Denuncia denuncia = new Denuncia();
            try
            {
                if (ModelState.IsValid)
                {
                    sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                    denuncia = (Denuncia)Session["Denuncia"];

                    DenunciaComentarioInputDto denunciaComentarioInputDto = new DenunciaComentarioInputDto()
                    {
                        IdDenuncia = entity.IdDenuncia,
                        IdUsuario = sesionUsuario.UsuarioId,
                        Comentario = entity.Comentario,
                        Fecha = util.FechaToIso(DateTime.Now.ToString()),
                        Activo = 1
                    };
                    var resultComentario = denunciaComentarioService.Insert(util.UrlServicios, sesionUsuario.Token, denunciaComentarioInputDto);

                    DenunciaEstadoInputDto denunciaEstado = new DenunciaEstadoInputDto()
                    {
                        IdDenuncia = entity.IdDenuncia,
                        IdEstadoDenuncia = entity.IdEstadoDenuncia

                    };
                    var resultEstado = denunciaService.UpdateEstado(util.UrlServicios, sesionUsuario.Token, denunciaEstado);
                    if (resultEstado != null)
                    {
                        if (resultEstado.Respuesta.CodigoRetorno == 1)
                        {
                            sesionUsuario.Token = resultEstado.Respuesta.Token;
                            if (denuncia.IdEstadoDenuncia != denunciaEstado.IdEstadoDenuncia)
                            {
                                if (denunciaEstado.IdEstadoDenuncia.ToString() == util.EstadoIniciada)
                                {
                                    EnvioCorreo("InicioDenunciante");
                                }
                                else if (denunciaEstado.IdEstadoDenuncia.ToString() == util.EstadoInsuficiente)
                                {
                                    EnvioCorreo("FaltaAntecedentes");
                                }
                                if (denunciaEstado.IdEstadoDenuncia.ToString() == util.EstadoMedidaDisciplinaria)
                                {
                                    EnvioCorreo("ConstituyeDelito");
                                }
                                if (denunciaEstado.IdEstadoDenuncia.ToString() == util.EstadoCerrado)
                                {
                                    EnvioCorreo("ResolucionDelito");
                                }
                                if (denunciaEstado.IdEstadoDenuncia.ToString() == util.EstadoSinMedidaDisciplinaria)
                                {
                                    EnvioCorreo("CerradaSinMedida");
                                }
                            }
                            return Json(util.RespuestaJson(true, ModelState, "Datos guarda-dos con éxito", "/denuncia/Index"), JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch
            {

            }

            return Json(util.RespuestaJson(false, ModelState, "Ha ocurrido un error, Intente nuevamente. Si el error persiste, comuníquese con el administrador.", ""), JsonRequestBehavior.AllowGet);
        }
        #endregion

        private bool EnvioCorreo(string IdentificadorCorreo, List<UsuarioEmailOutputDto> listado = null)
        {
            bool enviado = false;
            Denuncia denuncia = new Denuncia();
           
            try
            {
                UsuarioDatosOutputDto resultadoDenunciante = new UsuarioDatosOutputDto();
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                denuncia = (Denuncia)Session["Denuncia"];
                switch (IdentificadorCorreo)
                {
                    case "IngresoDenunciante":
                        enviado = envioMail.EnviarMail(IdentificadorCorreo, sesionUsuario.Usuario, denuncia.Identificador, sesionUsuario.Email, sesionUsuario.Token);
                        break;
                    case "IngresoDenuncianteAnonimo":
                        enviado = envioMail.EnviarMail(IdentificadorCorreo, sesionUsuario.Usuario, denuncia.Identificador, sesionUsuario.CorreoAnonimo, sesionUsuario.Token);
                        break;
                    case "IngresoOficial":
                        var result = tipoDelitoService.SelectNombreByIdTipoDelito(util.UrlServicios, sesionUsuario.Token, denuncia.detalles.IdTipoDelito.ToString());
                        if (result != null)
                        {
                            var resultDelito = JsonConvert.DeserializeObject<TipoDelitoNombreOutputDto>(result.Resultado.Data.ToString());
                           

                            if (listado != null)
                            {
                                foreach (var usuario in listado)
                                {
                                    //var email = JsonConvert.DeserializeObject<UsuarioEmailOutputDto>(usuario.ToString());

                                    enviado = envioMail.EnviarMail(IdentificadorCorreo, denuncia.Identificador, resultDelito.Nombre, usuario.Email, sesionUsuario.Token);
                                }
                            }

                            //var resultOC = correoService.EmailByIdRol(util.UrlServicios, sesionUsuario.Token, util.RolOficialCumplimiento);

                            //if (resultOC != null)
                            //{
                            //    foreach (var usuario in resultOC.Resultado.Data)
                            //    {
                            //        var resultadoOc = JsonConvert.DeserializeObject<UsuarioEmailOutputDto>(usuario.ToString());

                            //        enviado = envioMail.EnviarMail(IdentificadorCorreo, denuncia.Identificador, resultDelito.Nombre, resultadoOc.Email, sesionUsuario.Token);
                            //    }
                            //}

                        }
                        break;
                    case "IngresoSuperOficial":
                        var resultSO = tipoDelitoService.SelectNombreByIdTipoDelito(util.UrlServicios, sesionUsuario.Token, denuncia.detalles.IdTipoDelito.ToString());
                        if (resultSO != null)
                        {
                            var resultDelito = JsonConvert.DeserializeObject<TipoDelitoNombreOutputDto>(resultSO.Resultado.Data.ToString());
                            var resultSOC = correoService.EmailByIdRol(util.UrlServicios, sesionUsuario.Token, util.RolSuperOficial);

                            if (resultSOC!= null)
                            {
                                foreach (var usuario in resultSOC.Resultado.Data)
                                {
                                    var resultadoOc = JsonConvert.DeserializeObject<UsuarioEmailOutputDto>(usuario.ToString());

                                    enviado = envioMail.EnviarMail(IdentificadorCorreo, denuncia.Identificador, resultDelito.Nombre, resultadoOc.Email, sesionUsuario.Token);
                                }
                            }

                        }
                        break;
                    case "IngresoOficialBackup":
                        var resultBu = tipoDelitoService.SelectNombreByIdTipoDelito(util.UrlServicios, sesionUsuario.Token, denuncia.detalles.IdTipoDelito.ToString());
                        if (resultBu != null)
                        {
                            var resultDelito = JsonConvert.DeserializeObject<TipoDelitoNombreOutputDto>(resultBu.Resultado.Data.ToString());
                            var resultBC = correoService.EmailByBackup(util.UrlServicios, sesionUsuario.Token);

                            if (resultBC != null)
                            {
                                foreach (var usuario in resultBC.Resultado.Data)
                                {
                                    var resultadoOc = JsonConvert.DeserializeObject<UsuarioEmailOutputDto>(usuario.ToString());

                                    enviado = envioMail.EnviarMail(IdentificadorCorreo, denuncia.Identificador, resultDelito.Nombre, resultadoOc.Email, sesionUsuario.Token);
                                }
                            }

                        }
                        break;
                    case "InicioDenunciante":
                      

                        var resultDenunciante = correoService.DatosDenunciante(util.UrlServicios, sesionUsuario.Token, denuncia.idDenuncia.ToString());
                        if (resultDenunciante != null)
                        {

                             resultadoDenunciante = JsonConvert.DeserializeObject<UsuarioDatosOutputDto>(resultDenunciante.Resultado.Data.ToString());
                            enviado = envioMail.EnviarMail(IdentificadorCorreo, resultadoDenunciante.Nombre, resultadoDenunciante.Denuncia, resultadoDenunciante.Email, sesionUsuario.Token);
                        }

                        break;
                    case "FaltaAntecedentes":
                     
                        var resultDenuncianteF = correoService.DatosDenunciante(util.UrlServicios, sesionUsuario.Token, denuncia.idDenuncia.ToString());
                        if (resultDenuncianteF != null)
                        {
                             resultadoDenunciante = JsonConvert.DeserializeObject<UsuarioDatosOutputDto>(resultDenuncianteF.Resultado.Data.ToString());
                            enviado = envioMail.EnviarMail(IdentificadorCorreo, resultadoDenunciante.Nombre, resultadoDenunciante.Denuncia, resultadoDenunciante.Email, sesionUsuario.Token);
                        }
                        break;
                    case "NuevosAntecedentesOficial":
                        var resultNOC = correoService.EmailByIdRol(util.UrlServicios, sesionUsuario.Token, util.RolOficialCumplimiento);

                        if (resultNOC != null)
                        {
                            foreach (var usuario in resultNOC.Resultado.Data)
                            {
                                var resultadoOc = JsonConvert.DeserializeObject<UsuarioEmailOutputDto>(usuario.ToString());

                                enviado = envioMail.EnviarMail(IdentificadorCorreo, denuncia.Identificador, "", resultadoOc.Email, sesionUsuario.Token);
                            }
                        }
                        break;

                    case "NuevosAntecedentesBackup":
                        var resultNBC = correoService.EmailByBackup(util.UrlServicios, sesionUsuario.Token);

                        if (resultNBC != null)
                        {
                            foreach (var usuario in resultNBC.Resultado.Data)
                            {
                                var resultadoOc = JsonConvert.DeserializeObject<UsuarioEmailOutputDto>(usuario.ToString());

                                enviado = envioMail.EnviarMail(IdentificadorCorreo, denuncia.Identificador, "", resultadoOc.Email, sesionUsuario.Token);
                            }
                        }
                        break;
                    case "NuevosAntecedentesSuperOficial":
                        var resultNSOC = correoService.EmailByIdRol(util.UrlServicios, sesionUsuario.Token, util.RolSuperOficial);

                        if (resultNSOC != null)
                        {
                            foreach (var usuario in resultNSOC.Resultado.Data)
                            {
                                var resultadoOc = JsonConvert.DeserializeObject<UsuarioEmailOutputDto>(usuario.ToString());

                                enviado = envioMail.EnviarMail(IdentificadorCorreo, denuncia.Identificador, "", resultadoOc.Email, sesionUsuario.Token);
                            }
                        }
                        break;
                    case "ConstituyeDelito":
                        string conCopia = "";
                        var resultMailCE = correoService.EmailByIdRol(util.UrlServicios, sesionUsuario.Token, util.RolComiteEtica);
                        if (resultMailCE != null)
                        {
                            foreach (var email in resultMailCE.Resultado.Data)
                            {
                                var resultado = JsonConvert.DeserializeObject<UsuarioEmailOutputDto>(email.ToString());
                                if (denuncia.involucrados.Where(x => x.idUsuario == resultado.IdUsuario).Count() == 0)
                                {
                                    if (resultado.Email != "")
                                    {
                                        conCopia = conCopia + resultado.Email + ";";
                                    }
                                }
                                
                            }
                            conCopia = conCopia.TrimEnd(';');
                        }
                        
                        var resultDenuncianteCD = correoService.DatosDenunciante(util.UrlServicios, sesionUsuario.Token, denuncia.idDenuncia.ToString());
                        if (resultDenuncianteCD != null)
                        {
                            resultadoDenunciante = JsonConvert.DeserializeObject<UsuarioDatosOutputDto>(resultDenuncianteCD.Resultado.Data.ToString());
                           
                        }

                        enviado = envioMail.EnviarMail(IdentificadorCorreo,  resultadoDenunciante.Nombre, resultadoDenunciante.Denuncia, resultadoDenunciante.Email, sesionUsuario.Token,conCopia);
                        break;
                    case "CerradaSinMedida":
                        string conCopiaSinMedida = "";
                        var resultMailC = correoService.EmailByIdRol(util.UrlServicios, sesionUsuario.Token, util.RolComiteEtica);
                        if (resultMailC != null)
                        {
                            foreach (var email in resultMailC.Resultado.Data)
                            {
                                var resultado = JsonConvert.DeserializeObject<UsuarioEmailOutputDto>(email.ToString());
                                if (denuncia.involucrados.Where(x => x.idUsuario == resultado.IdUsuario).Count() == 0)
                                {
                                    if (resultado.Email != "")
                                    {
                                        conCopiaSinMedida = conCopiaSinMedida + resultado.Email + ";";
                                    }
                                }
                               
                            }
                            conCopiaSinMedida = conCopiaSinMedida.TrimEnd(';');
                        }
                       
                        var resultDenuncianteSM = correoService.DatosDenunciante(util.UrlServicios, sesionUsuario.Token, denuncia.idDenuncia.ToString());
                        if (resultDenuncianteSM != null)
                        {
                            resultadoDenunciante = JsonConvert.DeserializeObject<UsuarioDatosOutputDto>(resultDenuncianteSM.Resultado.Data.ToString());

                        }

                        enviado = envioMail.EnviarMail(IdentificadorCorreo, resultadoDenunciante.Nombre,  resultadoDenunciante.Denuncia,resultadoDenunciante.Email, sesionUsuario.Token, conCopiaSinMedida);
                        break;
                };


            }
            catch 
            {
                return enviado;
            }

            return enviado;
        }

        public FileResult Exportar(string Denuncia, string FechaDenuncia, string FechaOcurrencia, string IdSede, string IdDepartamento, string IdTipoDelito, string IdEstado)
        {
            var miCSV = new Jitbit.Utils.CsvExport();
            var filename = "Denuncias.csv";
            try
            {
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                string idUsuario = "";
                string inicioDenuncia = "", finDenuncia = "", inicioOcurrencia = "", finOcurrencia = "";
                if (FechaDenuncia != null)
                {
                    if (FechaDenuncia.Length > 0)
                    {
                        string[] fechas = FechaDenuncia.Split('-');
                        inicioDenuncia = util.FechaToIso(fechas[0].ToString().Trim());
                        finDenuncia = util.FechaToIso(fechas[1].ToString().Trim());
                    }
                }

                if (FechaOcurrencia != null)
                {
                    if (FechaOcurrencia.Length > 0)
                    {
                        string[] fechas = FechaOcurrencia.Split('-');
                        inicioOcurrencia = util.FechaToIso(fechas[0].ToString().Trim());
                        finOcurrencia = util.FechaToIso(fechas[1].ToString().Trim());
                    }
                }
                if (sesionUsuario.IdRol.ToString() == "3")
                {
                    idUsuario = sesionUsuario.IdRol.ToString();
                }
                var result = denunciaService.SelectReporteList(util.UrlServicios, sesionUsuario.Token, Denuncia, inicioDenuncia, finDenuncia, inicioOcurrencia, finOcurrencia, IdSede, IdDepartamento, IdTipoDelito, IdEstado, idUsuario);
                if (result != null)
                {
                    if (result.Respuesta.CodigoRetorno == 1)
                    {
                        if (result.Resultado.Data.Count > 0)
                        {
                            foreach (var item in result.Resultado.Data)
                            {
                                miCSV.AddRow();
                                var resultDenuncia = JsonConvert.DeserializeObject<DenunciaReporteListOutputDto>(item.ToString());
                                miCSV["IdDenuncia"] = resultDenuncia.Denuncia;
                                miCSV["Denunciante"] = resultDenuncia.Denunciante;
                                miCSV["Fecha"] = resultDenuncia.FechaIngreso;
                                miCSV["Denunciados"] = resultDenuncia.Denunciados;
                                miCSV["FechaDesde"] = resultDenuncia.FechaDesde;
                                miCSV["FechaHasta"] = resultDenuncia.FechaHasta;
                                miCSV["Sede"] = resultDenuncia.Sede;
                                miCSV["Departamento"] = resultDenuncia.Departamento;
                                miCSV["TipoDelito"] = resultDenuncia.TipoDelito;
                                miCSV["Estado"] = resultDenuncia.Estado;
                                miCSV["Dias"] = resultDenuncia.Diferencia;
                            }
                        }
                    }
                }

            }
            catch
            {

            }

            Response.Charset = "utf-8";
            return File(miCSV.ExportToBytes(), "application/csv", filename);
        }

        public int ValidaCambioEstado(int idDenuncia)
        {
            int modificar = 0;
            try
            { 
                if (Session["SesionActiva"] != null)
                {
                    sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                }

                if(sesionUsuario.IdRol.ToString() == util.RolOficialCumplimiento)
                {
                    var result = denunciaInvolucradoService.SelectInvolucradoDenuncia(util.UrlServicios, sesionUsuario.Token, idDenuncia, sesionUsuario.UsuarioId);
                    if(result != null)
                    {
                        if(result.Respuesta.CodigoRetorno == 1)
                        {
                            if (result.Resultado.Data == 0) modificar = 1;
                           //modificar = result.Resultado.Data;
                        }
                    }
                }
                else if(sesionUsuario.IdRol.ToString() == util.RolComiteEtica)
                {
                    var resultBackup = usuarioService.UsuarioBackup(util.UrlServicios, sesionUsuario.Token, sesionUsuario.UsuarioId);
                    if(resultBackup != null)
                    {
                        if(resultBackup.Respuesta.CodigoRetorno == 1)
                        {
                            if((int)resultBackup.Resultado.Data == 1)
                            {
                                var resultOficialInv = denunciaInvolucradoService.SelectInvolucradoRolDenuncia(util.UrlServicios, sesionUsuario.Token, idDenuncia);
                                if(resultOficialInv != null)
                                {
                                    if(resultOficialInv.Respuesta.CodigoRetorno == 1)
                                    {
                                        if(resultOficialInv.Resultado.Data > 0)
                                        {
                                            var result = denunciaInvolucradoService.SelectInvolucradoDenuncia(util.UrlServicios, sesionUsuario.Token, idDenuncia, sesionUsuario.UsuarioId);
                                            if (result != null)
                                            {
                                                if (result.Respuesta.CodigoRetorno == 1)
                                                {
                                                    if (result.Resultado.Data == 0) modificar = 1;
                                                }
                                            }
                                        }
                                    }
                                }
                                
                                
                            }
                        }
                    }
                }
                else if(sesionUsuario.IdRol.ToString() == util.RolSuperOficial)
                {
                    modificar = 1;
                }
            }
            catch
            { }
            return modificar;
        }
    }
}
