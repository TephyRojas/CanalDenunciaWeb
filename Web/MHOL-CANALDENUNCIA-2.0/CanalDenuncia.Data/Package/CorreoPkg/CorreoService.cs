using CanalDenunciaWeb.Data.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace CanalDenunciaWeb.Data.Package.CorreoPkg
{
    public class CorreoService
    {
        public RespuestaByIdOutputDto CorreoByIdentificador(string url, string token, string identificador)
        {
            var salida = new RespuestaByIdOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("Correo/CorreoByIdentificador", Method.GET);
            request.AddHeader("Authorization", token);
            request.AddParameter("identificador", identificador);

            request.RequestFormat = DataFormat.Json;
            var response = client.Execute<HttpStatusCode>(request);

            if (response.StatusCode == 0)
            {
                return null;
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent)
            {
                return null;
            }

            salida = JsonConvert.DeserializeObject<RespuestaByIdOutputDto>(response.Content.ToString());
            return salida;
        }

        public RespuestaByIdOutputDto CorreoByIdentificadorFuera(string url, string token, string identificador)
        {
            var salida = new RespuestaByIdOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("Correo/CorreoByIdentificadorFuera", Method.GET);
            request.AddHeader("Authorization", token);
            request.AddParameter("identificador", identificador);

            request.RequestFormat = DataFormat.Json;
            var response = client.Execute<HttpStatusCode>(request);

            if (response.StatusCode == 0)
            {
                return null;
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent)
            {
                return null;
            }

            salida = JsonConvert.DeserializeObject<RespuestaByIdOutputDto>(response.Content.ToString());
            return salida;
        }
        public RespuestaByIdOutputDto DatosDenunciante(string url, string token, string idDenuncia)
        {
            var salida = new RespuestaByIdOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("DatosEnvioCorreo/DatosDenuncianteByIdDenuncia", Method.GET);
            request.AddHeader("Authorization", token);
            request.AddParameter("IdDenuncia", idDenuncia);

            request.RequestFormat = DataFormat.Json;
            var response = client.Execute<HttpStatusCode>(request);

            if (response.StatusCode == 0)
            {
                return null;
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent)
            {
                return null;
            }

            salida = JsonConvert.DeserializeObject<RespuestaByIdOutputDto>(response.Content.ToString());
            return salida;
        }

        public RespuestaListOutputDto EmailByIdRol(string url, string token, string idRol)
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("DatosEnvioCorreo/EmailByIdRolList", Method.GET);
            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;


            if (idRol.Length > 0)
            {
                if (idRol != "0")
                {
                    request.AddParameter("idRol", idRol);
                }
            }


            var response = client.Execute<HttpStatusCode>(request);

            if (response.StatusCode == 0)
            {
                return null;
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent)
            {
                return null;
            }

            salida = JsonConvert.DeserializeObject<RespuestaListOutputDto>(response.Content.ToString());
            return salida;
        }

        public RespuestaListOutputDto EmailByBackup(string url, string token)
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("DatosEnvioCorreo/EmailByBackup", Method.GET);
            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;


         

            var response = client.Execute<HttpStatusCode>(request);

            if (response.StatusCode == 0)
            {
                return null;
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent)
            {
                return null;
            }

            salida = JsonConvert.DeserializeObject<RespuestaListOutputDto>(response.Content.ToString());
            return salida;
        }

    }
}
