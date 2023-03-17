using CanalDenunciaWeb.Data.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace CanalDenunciaWeb.Data.Package.UsuarioRolPkg
{
    public class UsuarioRolService
    {
        public RespuestaListOutputDto SelectUsuarioRolByIdRol(string url, string token, string rol)
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("UsuarioRol/UsuarioRolByIdRol", Method.GET);
            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("idRol", rol);
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

        public RespuestaListOutputDto ListRolByIdUsuario(string url, string token, string idUsuario)
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("UsuarioRol/UsuarioRolByIdUsuario", Method.GET);
            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("idUsuario", idUsuario);
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

        public RespuestaListOutputDto SelectUsuarioRolByBackup(string url, string token)
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("UsuarioRol/UsuarioRolByBackUp", Method.GET);
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

        public RespuestaListOutputDto SelectListCombobox(string url, string token, string idUsuario)
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("UsuarioRol/UsuarioRolByIdUsuario", Method.GET);
            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("idUsuario", idUsuario);
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
