using CanalDenunciaServicio.Models;
using CanalDenunciaServicio.Security;
using NLog;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CanalDenunciaServicio.Helper
{
    public class Util
    {
        private Logger logger = LogManager.GetLogger("logSistema");
        private const string initVector = "#F4ct0r1ngS3cur1ty*";
        private const int keysize = 256;

        #region "Webconfig"
        private string conn = ConfigurationManager.ConnectionStrings["conn"].ToString();
        private string noData = ConfigurationManager.AppSettings["NoData"].ToString();
        private string error = ConfigurationManager.AppSettings["Error"].ToString();
        private string errorGenerico = ConfigurationManager.AppSettings["ErrorGenerico"].ToString();
        private string validaToken = ConfigurationManager.AppSettings["validaToken"].ToString();
        private string secretKey = ConfigurationManager.AppSettings["secretKey"].ToString();
        private string duracionToken = ConfigurationManager.AppSettings["duracionToken"].ToString();

        public string Conn { get => conn; set => conn = value; }
        public string NoData { get => noData; set => noData = value; }
        public string Error { get => error; set => error = value; }
        public string ErrorGenerico { get => errorGenerico; set => errorGenerico = value; }
        public string ValidaToken { get => validaToken; set => validaToken = value; }
        public string SecretKey { get => secretKey; set => secretKey = value; }
        public string DuracionToken { get => duracionToken; set => duracionToken = value; }

        #endregion

        public string TimeStamp(int formatoIso = 0)
        {
            string Salida = "";

            if (formatoIso == 0)
            {
                Salida = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString();
            }
            else
            {
                Salida = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString();
            }

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
                string salida = DateTime.Parse(Fecha).Year.ToString() + "-" + DateTime.Parse(Fecha).Month.ToString() + "-" + DateTime.Parse(Fecha).Day.ToString();
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

        public bool ValidaExpresion(string ExpresionRegular, string Valor)
        {
            Regex rx = new Regex(ExpresionRegular);

            try
            {

                if (rx.IsMatch(Valor))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }

        public void logSistema(string mensaje, int? idTipo = 0, string controlador = "", string action = "", string usuario = "", string rutEmpresa = "", string ip = "")
        {
            switch (idTipo)
            {
                case 1: //Info
                    logger.Info(mensaje);
                    break;
                case 2: //Error
                    logger.Error(mensaje);
                    break;
                case 0:
                    logger.Fatal(mensaje);
                    break;
                default:
                    logger.Fatal(mensaje);
                    break;
            }
        }

        public int? GetIso8601WeekOfYear(DateTime time)
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

        public string RutFormat(string rutSinFormato)
        {
            if (rutSinFormato == null || rutSinFormato.Length < 2)
            {
                return "";
            }

            string rutTemporal = rutSinFormato.Substring(0, rutSinFormato.Length - 1);
            string dv = rutSinFormato.Substring(rutSinFormato.Length - 1, 1);

            if (!long.TryParse(rutTemporal, out long rut))
            {
                rut = 0;
            }

            string rutFormateado = rut.ToString("N0");

            if (rutFormateado.Equals("0"))
            {
                rutFormateado = "asdasd";
            }
            else
            {
                rutFormateado += "-" + dv;
                rutFormateado = rutFormateado.Replace(",", ".");
            }

            return rutFormateado;
        }

        public string Encrypt(string plainText, string passPhrase = "")
        {
            passPhrase = "#F4ct0r1ngS3cur1ty*";
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            String b = Convert.ToBase64String(cipherTextBytes);
            return Convert.ToBase64String(cipherTextBytes);
        }

        public string Decrypt(string cipherText, string passPhrase = "")
        {
            passPhrase = "#F4ct0r1ngS3cur1ty*";
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            string a = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }


        public string RenuevaToken(HttpRequestHeaders header)
        {
            if (header.Authorization != null)
            {
                if (header.Authorization.ToString().Length > 0)
                {
                    if (ValidaToken == "1")
                    {
                        var token = TokenValidation.ValidaToken(header.Authorization.ToString());

                        if (token != null)
                        {
                            var tokenOuput = TokenGenerator.GenerateTokenJwt(token.usuario, token.email, token.nombreUsuario, Convert.ToInt32(DuracionToken));
                            return tokenOuput;
                        }
                    }
                    else
                    {
                        return Guid.NewGuid().ToString();
                    }
                }
            }

            return null;

        }

        public RespuestaGenericaOutputDto respuestaGenerica(int codigo, string mensaje, string token, object entity = null)
        {
            RespuestaGenericaOutputDto respuestaGenericaOutputDto = new RespuestaGenericaOutputDto();

            //Respuesta
            RespuestaGenericaOutputDto.respuesta respuesta = new RespuestaGenericaOutputDto.respuesta()
            {
                CodigoRetorno = codigo,
                Mensaje = mensaje,
                Token = token
            };

            //resultado
            RespuestaGenericaOutputDto.resultado resultado = new RespuestaGenericaOutputDto.resultado()
            {
                Data = entity
            };

            respuestaGenericaOutputDto.Respuesta = respuesta;
            respuestaGenericaOutputDto.Resultado = resultado;

            return respuestaGenericaOutputDto;

        }

    }
}