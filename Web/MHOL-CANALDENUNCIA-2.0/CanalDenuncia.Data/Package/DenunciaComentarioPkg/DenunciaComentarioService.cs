using CanalDenunciaWeb.Data.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace CanalDenunciaWeb.Data.Package.DenunciaComentarioPkg
{
    public class DenunciaComentarioService
    {
        public RespuestaOutputDto Insert(string url, string token, DenunciaComentarioInputDto entity)
        {
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaComentario/PostDenunciaComentario", Method.POST);

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

        public RespuestaOutputDto Delete(string url, string token, string idDenuncia)
        {
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaComentario/PostDenunciaComentario", Method.POST);

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

            var salida = JsonConvert.DeserializeObject<RespuestaOutputDto>(response.Content.ToString());

            return salida;
        }

        public RespuestaOutputDto SelectById(string url, string token, int id)
        {
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaComentario/SelectById", Method.POST);

            request.AddHeader("Authorization", token);
            request.AddParameter("Id", id);
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
