using CanalDenunciaWeb.Data.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaWeb.Data.Package.DenunciaCorreoAnonimoPkg
{
    public class DenunciaCorreoAnonimoService
    {
        public RespuestaOutputDto Insert(string url, string token, DenunciaCorreoAnonimoInputDto entity)
        {
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaCorreoAnonimo/PostDenunciaCorreoAnonimo", Method.POST);

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

        public RespuestaOutputDto SelectById(string url, string token, int id)
        {
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaCorreoAnonimo/DenunciaCorreoAnonimoByIdDenuncia", Method.POST);

            request.AddHeader("Authorization", token);
            request.AddParameter("IdDenuncia", id);
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
