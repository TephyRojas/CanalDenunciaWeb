using CanalDenunciaWeb.Data.Package.DepartamentoSedePkg;
using CanalDenunciaWeb.Data.Package.UsuarioPkg;
using CanalDenunciaWeb.Helper;
using CanalDenunciaWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CanalDenunciaWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private Util util = new Util();
        private EnvioMail envioMail = new EnvioMail();
        private SesionUsuarioDto sesionUsuario = new SesionUsuarioDto();
        private DepartamentoSedeService departamentoSedeService = new DepartamentoSedeService();
        private UsuarioService usuarioService = new UsuarioService();
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CargarArchivo(HttpPostedFileBase file)
        {
            string rut = "";
            try
            {
                List<UsuarioInputDto> usuarioInputDto = new List<UsuarioInputDto>();
                sesionUsuario = (SesionUsuarioDto)Session["SesionActiva"];
                
                if (sesionUsuario.IdRol.ToString() == util.RolOficialCumplimiento)
                {
                    if (file != null)
                    {
                        string extension = Path.GetExtension(file.FileName);
                        if (extension.ToUpper() == ".CSV")
                        {

                            using (StreamReader sr = new StreamReader(file.InputStream))
                            {
                                string[] contenido = new string[8];
                                sr.ReadLine();
                                //if (contenido.Length > 1)
                                //{
                                //    if (contenido[0] != null)
                                //    {
                                if (sr.Peek() != -1)
                                {
                                    while (sr.Peek() != -1)
                                    {
                                        contenido = sr.ReadLine().Split(';');

                                        //Valida que todos los registros tengan contenido
                                        if (contenido.Where(x => x.Length == 0).Count() >= 1)
                                        {
                                            if (contenido[0].Length > 1)
                                            {
                                                return Json(util.RespuestaJson(false, ModelState, "Debes completar todos los campos requeridos, Intente nuevamente. Rut: " + contenido[0].ToString(), ""), JsonRequestBehavior.AllowGet);
                                            }
                                            else
                                            {
                                                return Json(util.RespuestaJson(false, ModelState, "Debes completar todos los campos requeridos, Intente nuevamente.", ""), JsonRequestBehavior.AllowGet);
                                            }
                                        }
                                        rut = contenido[0].ToString();
                                        //Valida que el departamento/sede exista
                                        var result = departamentoSedeService.SelectExist(util.UrlServicios, sesionUsuario.Token, contenido[7].ToString(), contenido[6].ToString());
                                        if (result != null)
                                        {
                                            if (result.Respuesta.CodigoRetorno != 1)
                                            {
                                                return Json(util.RespuestaJson(false, ModelState, "Error en departamento o sede registro rut : " + rut, ""), JsonRequestBehavior.AllowGet);
                                            }
                                            else
                                            {
                                                UsuarioInputDto usuario = new UsuarioInputDto()
                                                {
                                                    Rut = contenido[0].Trim(),
                                                    Nombre = contenido[1],
                                                    PrimerApellido = contenido[2],
                                                    SegundoApellido = contenido[3],
                                                    FechaIngresoContrato = util.FechaToIso(contenido[4]),
                                                    FechaFinContrato = util.FechaToIso(contenido[5]),
                                                    idDepartamentoSede = (int)result.Resultado.Data
                                                };
                                                usuarioInputDto.Add(usuario);
                                            }
                                        }
                                    }
                                    //    }
                                    //    else
                                    //    {
                                    //        return Json(util.RespuestaJson(false, ModelState, "Archivo sin contenido", ""), JsonRequestBehavior.AllowGet);
                                    //    }
                                    //}
                                }
                                else
                                {
                                    return Json(util.RespuestaJson(false, ModelState, "Archivo sin contenido", ""), JsonRequestBehavior.AllowGet);
                                }

                            }


                        }
                        else
                        {
                            return Json(util.RespuestaJson(false, ModelState, "Debe cargar un archivo de extensión .csv", ""), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(util.RespuestaJson(false, ModelState, "Debe cargar un archivo de extensión .csv", ""), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    //no puede cargar
                };
                var resultUsuario = usuarioService.Insert(util.UrlServicios, sesionUsuario.Token, usuarioInputDto);
                if (resultUsuario != null)
                {
                    return Json(util.RespuestaJson(true, ModelState, "Datos guardados con éxito", "/usuario/index"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(util.RespuestaJson(false, ModelState, "Error en los datos ingresados, revise registro Rut : " +rut, ""), JsonRequestBehavior.AllowGet);
                }
              

            }
            catch(Exception ex)
            {
                return Json(util.RespuestaJson(false, ModelState, "Debes completar todos los campos requeridos, Intente nuevamente. Rut: " + rut, ""), JsonRequestBehavior.AllowGet);
            }
           
        }

        public FileResult Descargar()
        {
            string ruta = "";
            string[] regreso = new string[] { "" };

            ruta = Server.MapPath(util.RutaArchivo);
            var FileVirtualPath = ruta;

            return File(FileVirtualPath, "application/force- download", Path.GetFileName(FileVirtualPath));

        }
    }
}
