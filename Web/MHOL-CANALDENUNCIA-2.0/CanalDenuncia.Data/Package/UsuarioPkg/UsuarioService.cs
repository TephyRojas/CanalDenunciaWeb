using CanalDenunciaWeb.Data.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace CanalDenunciaWeb.Data.Package.UsuarioPkg
{
    public class UsuarioService
    {
        public RespuestaByIdOutputDto SelectLogin(string url, string token, UsuarioLoginInputDto entity)
        {
            var salida = new RespuestaByIdOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("Usuario/UsuarioLogin", Method.POST);
            request.AddHeader("Authorization", token);
            request.AddJsonBody(entity);
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

        public RespuestaByIdOutputDto SelectAnonimo(string url)
        {
            var salida = new RespuestaByIdOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("Usuario/UsuarioAnonimo", Method.POST);
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

        public RespuestaByIdOutputDto SelectUsuarioByRut(string url, string token, string rut)
        {
            var salida = new RespuestaByIdOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("Usuario/UsuarioByRut", Method.GET);
            request.AddHeader("Authorization", token);
            request.AddParameter("rut", rut);
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

        public RespuestaByIdOutputDto SelectUsuarioById(string url, string token, int id)
        {
            var salida = new RespuestaByIdOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("Usuario/UsuarioById", Method.GET);
            request.AddHeader("Authorization", token);
            request.AddParameter("id", id);
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

        public RespuestaByIdOutputDto SelectUsuarioByIdUsuario(string url, string token, int id)
        {
            var salida = new RespuestaByIdOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("Usuario/UsuarioByIdUsuario", Method.GET);
            request.AddHeader("Authorization", token);
            request.AddParameter("idUsuario", id);
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
        public RespuestaOutputDto UpdateContrasenaTemporal(string url, string token, UsuarioContrasenaTemporalInputDto entity)
        {
            var client = new RestClient(url);
            var request = new RestRequest("Usuario/PutUsuarioContrasenaTemporal", Method.POST);
            request.AddHeader("Authorization", token);
            request.AddJsonBody(entity);
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

            var salida = JsonConvert.DeserializeObject<RespuestaOutputDto>(response.Content.ToString());

            return salida;
        }

        public RespuestaOutputDto UpdateBloqueContrasena(string url, string token, UsuarioBloqueoPassowordInputDto entity)
        {
            var client = new RestClient(url);
            var request = new RestRequest("Usuario/PutUsuarioBloqueoContrasena", Method.POST);
            request.AddHeader("Authorization", token);
            request.AddJsonBody(entity);
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

            var salida = JsonConvert.DeserializeObject<RespuestaOutputDto>(response.Content.ToString());

            return salida;
        }

        public RespuestaOutputDto UpdateDatosPersonales(string url, string token, UsuarioDatosPersonalesInputDto entity)
        {
            var client = new RestClient(url);
            var request = new RestRequest("Usuario/PutUsuarioDatosPersonales", Method.POST);
            request.AddHeader("Authorization", token);
            request.AddJsonBody(entity);
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

            var salida = JsonConvert.DeserializeObject<RespuestaOutputDto>(response.Content.ToString());

            return salida;
        }

        public RespuestaOutputDto UsuarioBackup(string url, string token, int id)
        {
            var client = new RestClient(url);
            var request = new RestRequest("Usuario/UsuarioEsBackup", Method.GET);
            request.AddHeader("Authorization", token);
            request.AddParameter("id", id);
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

            var salida = JsonConvert.DeserializeObject<RespuestaOutputDto>(response.Content.ToString());

            return salida;
        }

        public RespuestaListOutputDto SelectListContrasena(string url, string token, int id)
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("Usuario/SelectListPasswordById", Method.GET);
            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("id", id);
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

        public RespuestaOutputDto Insert(string url, string token, List<UsuarioInputDto> entity)
        {
            var client = new RestClient(url);
            var request = new RestRequest("Usuario/PostUsuario", Method.POST);

            request.AddHeader("Authorization", token);
            request.AddJsonBody(entity);
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

            var salida = JsonConvert.DeserializeObject<RespuestaOutputDto>(response.Content.ToString());

            return salida;
        }
    }
}
