using CanalDenunciaWeb.Data.Package.CorreoPkg;
using Newtonsoft.Json;

namespace CanalDenunciaWeb.Helper
{
    public class EnvioMail
    {
        CorreoService correoService = new CorreoService();
        Util util = new Util();
        public bool EnviarMail(string IdentificadorCorreo, string parametro1, string parametro2, string mail, string token, string concopia = "")
        {

            bool envio = true;
            CorreoOutputDto resultCorreo = new CorreoOutputDto();
            var result = correoService.CorreoByIdentificador(util.UrlServicios, token, IdentificadorCorreo);
            if (result != null)
            {
                if (result.Respuesta.CodigoRetorno == 1)
                {
                    resultCorreo = JsonConvert.DeserializeObject<CorreoOutputDto>(result.Resultado.Data.ToString());
                    resultCorreo.Cuerpo = resultCorreo.Cuerpo.Replace(resultCorreo.Parametro.Split('|')[0], parametro1);
                    if (parametro2 != "")
                    {
                        resultCorreo.Cuerpo = resultCorreo.Cuerpo.Replace(resultCorreo.Parametro.Split('|')[1], parametro2);
                    }
                }
            }

            envio = util.SendMail(mail, resultCorreo.Asunto, resultCorreo.Cuerpo,concopia);

            return envio;
        }


    }
}