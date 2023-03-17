
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.Office.Interop.Outlook;

namespace CanalDenunciaWeb.Helper
{
    public class Util
    {
       
        //private Logger logger = LogManager.GetLogger("logSistema");
        //private Logger logger = LogManager.GetLogger("logSistema");
        private const int keysize = 256;

        #region "Webconfig"
        private string urlServicios = ConfigurationManager.AppSettings["UrlServicios"].ToString();
        private string rolOficialCumplimiento = ConfigurationManager.AppSettings["RolOficialCumplimiento"].ToString();
        private string rolComiteEtica = ConfigurationManager.AppSettings["RolComiteEtica"].ToString();
        private string rolDenunciante = ConfigurationManager.AppSettings["RolDenunciante"].ToString();
        private string rolSuperOficial = ConfigurationManager.AppSettings["RolSuperOficial"].ToString();
        private string estadoRecibida = ConfigurationManager.AppSettings["EstadoRecibida"].ToString();
        private string estadoIniciada = ConfigurationManager.AppSettings["EstadoIniciada"].ToString();
        private string estadoInsuficiente = ConfigurationManager.AppSettings["EstadoInsuficiente"].ToString();
        private string estadoMedidaDisciplinaria = ConfigurationManager.AppSettings["EstadoMedidaDisciplinaria"].ToString();
        private string estadoSinMedidaDisciplinaria = ConfigurationManager.AppSettings["EstadoSinMedidaDisciplinaria"].ToString();
        private string estadoCerrado = ConfigurationManager.AppSettings["EstadoCerrado"].ToString();
        private string usuarioAnonimo = ConfigurationManager.AppSettings["UsuarioAnonimo"].ToString();
        private string urlCaducaSession = ConfigurationManager.AppSettings["UrlCaducaSession"].ToString();
        private string extensiones = ConfigurationManager.AppSettings["ExtensionValidacion"].ToString();
        private string intentos = ConfigurationManager.AppSettings["CantidadIntentos"].ToString();
        private string tiempoBloqueo = ConfigurationManager.AppSettings["TiempoBloqueo"].ToString();
        private string tiempoCambioContrasena = ConfigurationManager.AppSettings["TiempoCambioContrasena"].ToString();
        private string textoLogin = ConfigurationManager.AppSettings["textoLogin"].ToString();
        private string rutaArchivo = ConfigurationManager.AppSettings["rutaArchivo"].ToString();
        private string correoRemitente = ConfigurationManager.AppSettings["correoRemitente"].ToString();
        public string UrlServicios { get => urlServicios; set => urlServicios = value; }
        public string RolOficialCumplimiento { get => rolOficialCumplimiento; set => rolOficialCumplimiento = value; }
        public string RolComiteEtica { get => rolComiteEtica; set => rolComiteEtica = value; }
        public string RolDenunciante { get => rolDenunciante; set => rolDenunciante = value; }
        public string EstadoRecibida { get => estadoRecibida; set => estadoRecibida = value; }
        public string EstadoIniciada { get => estadoIniciada; set => estadoIniciada = value; }
        public string EstadoInsuficiente { get => estadoInsuficiente; set => estadoInsuficiente = value; }
        public string EstadoMedidaDisciplinaria { get => estadoMedidaDisciplinaria; set => estadoMedidaDisciplinaria = value; }
        public string EstadoSinMedidaDisciplinaria { get => estadoSinMedidaDisciplinaria; set => estadoSinMedidaDisciplinaria = value; }
        public string EstadoCerrado { get => estadoCerrado; set => estadoCerrado = value; }
        public string UsuarioAnonimo { get => usuarioAnonimo; set => usuarioAnonimo = value; }
        public string UrlCaducaSession { get => urlCaducaSession; set => urlCaducaSession = value; }
        public string Extensiones { get => extensiones; set => extensiones = value; }
        public string Intentos { get => intentos; set => intentos = value; }
        public string TiempoBloqueo { get => tiempoBloqueo; set => tiempoBloqueo = value; }
        public string TiempoCambioContrasena { get => tiempoCambioContrasena; set => tiempoCambioContrasena = value; }
        public string RolSuperOficial { get => rolSuperOficial; set => rolSuperOficial = value; }
        public string TextoLogin { get => textoLogin; set => textoLogin = value; }
        public string RutaArchivo { get => rutaArchivo; set => rutaArchivo = value; }
        public string CorreoRemitente { get => correoRemitente; set => correoRemitente = value; }
        #endregion

        public string TimeStamp()
        {
            string Salida = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString();
            return Salida;
        }

        public string FechaATimeStamp(string Fecha)
        {

            if (Fecha.Length > 0)
            {
                string salida = DateTime.Parse(Fecha).Year.ToString() + "-" + DateTime.Parse(Fecha).Month.ToString() + "-" + DateTime.Parse(Fecha).Day.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString();
                return salida;
            }
            else
            {
                return Fecha;
            }
        }

        public string FechaToIso(string Fecha)
        {

            if (Fecha.Length > 0)
            {
                string mes = ""; string dia = "";
                if (DateTime.Parse(Fecha).Month.ToString().Length == 1)
                {
                    mes = "0" + DateTime.Parse(Fecha).Month.ToString();
                }
                else
                {
                    mes = DateTime.Parse(Fecha).Month.ToString();
                }
                if (DateTime.Parse(Fecha).Day.ToString().Length == 1)
                {
                    dia = "0" + DateTime.Parse(Fecha).Day.ToString();
                }
                else
                {
                    dia = DateTime.Parse(Fecha).Day.ToString();
                }
                string salida = DateTime.Parse(Fecha).Year.ToString() + "-" + mes + "-" + dia;
                return salida;
            }
            else
            {
                return Fecha;
            }
        }

        public string FechaToIsoHour(string Fecha)
        {

            if (Fecha.Length > 0)
            {
                string mes = ""; string dia = "";
                if (DateTime.Parse(Fecha).Month.ToString().Length == 1)
                {
                    mes = "0" + DateTime.Parse(Fecha).Month.ToString();
                }
                else
                {
                    mes = DateTime.Parse(Fecha).Month.ToString();
                }
                if (DateTime.Parse(Fecha).Day.ToString().Length == 1)
                {
                    dia = "0" + DateTime.Parse(Fecha).Day.ToString();
                }
                else
                {
                    dia = DateTime.Parse(Fecha).Day.ToString();
                }
                string salida = DateTime.Parse(Fecha).Year.ToString() + "-" + mes + "-" + dia+' '+DateTime.Parse(Fecha).Hour+":"+DateTime.Parse(Fecha).Minute+":"+DateTime.Parse(Fecha).Second;
                return salida;
            }
            else
            {
                return Fecha;
            }
        }

        public string FechaToDayMonthYear(string Fecha)
        {

            if (Fecha.Length > 0)
            {
                string mes = ""; string dia = "";
                if (DateTime.Parse(Fecha).Month.ToString().Length == 1)
                {
                    mes = "0" + DateTime.Parse(Fecha).Month.ToString();
                }
                else
                {
                    mes = DateTime.Parse(Fecha).Month.ToString();
                }
                if (DateTime.Parse(Fecha).Day.ToString().Length == 1)
                {
                    dia = "0" + DateTime.Parse(Fecha).Day.ToString();
                }
                else
                {
                    dia = DateTime.Parse(Fecha).Day.ToString();
                }
                string salida = dia + "/" + mes + "/" + DateTime.Parse(Fecha).Year;
                return salida;
            }
            else
            {
                return Fecha;
            }
        }


        public Boolean IsNumeric(string Valor)
        {
            double Num;

            try
            {

                bool isNum = double.TryParse(Valor, out Num);

                if (isNum)
                    return true;
                else
                    return false;

            }
            catch
            {
                return false;
            }
        }

        //public void logSistema(string mensaje, int? idTipo = 0)
        //{
        //    switch (idTipo)
        //    {
        //        case 1: //Info
        //            logger.Info(mensaje);
        //            break;
        //        case 2: //Error
        //            logger.Error(mensaje);
        //            break;
        //        case 0:
        //            logger.Fatal(mensaje);
        //            break;
        //        default:
        //            logger.Fatal(mensaje);
        //            break;
        //    }
        //}

        public jsonReturn RespuestaJson(bool estado, ModelStateDictionary modelState, string errorMessage, string Redirect)
        {
            var respuesta = new jsonReturn();
            List<string> errorControl = new List<string>();
            string error = "";

            foreach (var item in modelState)
            {
                if (item.Value.Errors.Count > 0)
                {
                    error = item.Key.ToString() + "|" + item.Value.Errors[0].ErrorMessage.ToString();
                }
                else
                {
                    error = item.Key.ToString() + "|NOERROR";
                }

                errorControl.Add(error);
            }

            respuesta.errorSumary = modelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            respuesta.respuesta = estado;
            respuesta.errorMessage = errorMessage;
            respuesta.errorControl = errorControl;
            respuesta.redirect = Redirect;

            return respuesta;

        }

        public int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }

        public jsonReturn ContrasenasInvalidasJson(bool estado, ModelStateDictionary modelState, string errorMessage, string Redirect)
        {
            var respuesta = new jsonReturn();
            List<string> errorControl = new List<string>();
            string error = "";

            foreach (var item in modelState)
            {
                if (item.Key.ToString() == "clave")
                {
                    error = item.Key.ToString() + "|Las contraseñas no son iguales.";
                    errorControl.Add(error);
                }
                else if (item.Key.ToString() == "repitaClave")
                {
                    error = item.Key.ToString() + "|Las contraseñas no son iguales.";
                    errorControl.Add(error);
                }
                else
                {
                    error = item.Key.ToString() + "|NOERROR";
                    errorControl.Add(error);
                }
            }

            respuesta.errorSumary = modelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            respuesta.respuesta = estado;
            respuesta.errorMessage = errorMessage;
            respuesta.errorControl = errorControl;
            respuesta.redirect = Redirect;

            return respuesta;

        }

        public Boolean ValidarEmail(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public jsonReturn CorreoInvalidoJson(bool estado, ModelStateDictionary modelState, string errorMessage, string Redirect)
        {
            var respuesta = new jsonReturn();
            List<string> errorControl = new List<string>();
            string error = "";

            foreach (var item in modelState)
            {
                if (item.Key.ToString() == "email")
                {
                    error = item.Key.ToString() + "|El correo ingresado es invalido.";
                    errorControl.Add(error);
                }
                else
                {
                    error = item.Key.ToString() + "|NOERROR";
                    errorControl.Add(error);
                }
            }

            respuesta.errorSumary = modelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            respuesta.respuesta = estado;
            respuesta.errorMessage = errorMessage;
            respuesta.errorControl = errorControl;
            respuesta.redirect = Redirect;

            return respuesta;

        }

        public string Encriptar(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted =
            System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        /// Esta función "desencripta" la cadena que le envíamos en el parámentro de entrada.
        public static string DesEncriptar(string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted =
            Convert.FromBase64String(_cadenaAdesencriptar);
            //result = 
            System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }

        public bool SendMail(string to, string Subject, string body, string copia = "")
        {
            try
            {
                var Mensaje = new MailMessage();
                Mensaje.To.Add(new MailAddress(to));
                Mensaje.From = new MailAddress(correoRemitente);
                Mensaje.Subject = Subject;
                Mensaje.Body = body;
                Mensaje.IsBodyHtml = true;
                if(copia != "")
                {
                    string[] CorreoCopia = copia.Split(';');
                    foreach(string x in CorreoCopia)
                    {
                        Mensaje.Bcc.Add(new MailAddress(x));
                       
                    }
                   
                }
                using (var smtp = new SmtpClient())
                {
                   
                    smtp.Host = WebConfigurationManager.AppSettings["SMTPHost"];
                    smtp.Port = int.Parse(WebConfigurationManager.AppSettings["SMTPPort"]);
                    smtp.Credentials = new NetworkCredential(WebConfigurationManager.AppSettings["SMTPName"], WebConfigurationManager.AppSettings["SMTPPassword"]);
                    smtp.EnableSsl = true;
                   
                    smtp.Send(Mensaje);
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                
            }
            return false;
        }


        //public  Boolean SendEmailWithOutlook(string mailDirection, string mailSubject, string maiLContent)
        //{
        //    try
        //    {
        //        var oApp = new Microsoft.Office.Interop.Outlook.Application();

        //        Microsoft.Office.Interop.Outlook.NameSpace ns = oApp.GetNamespace("MAPI");
        //        var f = ns.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderInbox);

        //        System.Threading.Thread.Sleep(1000);

        //        var mailItem = (Microsoft.Office.Interop.Outlook.MailItem)oApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);

        //        mailItem.Subject = mailSubject;
        //        mailItem.HTMLBody = maiLContent;
        //        mailItem.To = mailDirection;
        //        mailItem.Send();

        //    }
        //    catch (System.Exception ex)
        //    {
        //        return false;
        //    }
        //    finally
        //    {
        //    }
        //    return true;
        //}


        public string GenerarPassword(int longitud)
        {
           
            using (var crypto = new RNGCryptoServiceProvider())
            {
                var bits = (longitud * 6);
                var byte_size = ((bits + 7) / 8);
                var bytesarray = new byte[byte_size];
                crypto.GetBytes(bytesarray);
                return Convert.ToBase64String(bytesarray);
            }
            //return contraseña;
        }

        public int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public string ContrasenaSegura(String contraseñaSinVerificar)
        {
           
            string msj = "";
            //letras de la A a la Z, mayusculas y minusculas
            Regex letras = new Regex(@"[A-Z]");
            Regex letrasM = new Regex(@"[a-z]");
            //digitos del 0 al 9
            Regex numeros = new Regex(@"[0-9]");
            //cualquier caracter del conjunto
            Regex caracEsp = new Regex("[!\"#\\$%&'()*+,-./:;=?@\\[\\]^_`{|}~]");

            //si no contiene las letras, regresa false
            if (!letras.IsMatch(contraseñaSinVerificar))
            {
                msj += "<p> Al menos una letra Mayuscula </p>";

            }
            //si no contiene las letras, regresa false
            if (!letrasM.IsMatch(contraseñaSinVerificar))
            {
                msj += " <p> Al menos una letra Minúscula </p>";
            }
            //si no contiene los numeros, regresa false
            if (!numeros.IsMatch(contraseñaSinVerificar))
            {
                msj += " <p> Al menos un número </p>";
            }

            //si no contiene los caracteres especiales, regresa false
            if (!caracEsp.IsMatch(contraseñaSinVerificar))
            {
                msj += " <p> Al menos un caracter especial </p> ";
            }

            
            
            return msj;
}

    }

}