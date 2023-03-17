using CanalDenunciaWeb.Data.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace CanalDenunciaWeb.Data.Package.DenunciaDocumentoPkg
{
    public class DenunciaDocumentoService
    {

        public RespuestaListOutputDto SelectByIdDenuncia(string url, string token, string Denuncia)
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaDocumento/DenunciaDocumentoByIdDenuncia", Method.GET);
            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;

            if (Denuncia.Length > 0)
            {
                request.AddParameter("id", Denuncia);
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
        public RespuestaOutputDto Insert(string url, string token, DenunciaDocumentoInputDto entity)
        {
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaDocumento/PostDenunciaDocumento", Method.POST);

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

        public RespuestaOutputDto Update(string url, string token, DenunciaDocumentoInputDto entity)
        {
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaDocumento/PutDenunciaDocumento", Method.POST);

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

        public RespuestaOutputDto Delete(string url, string token, int Id)
        {
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaDocumento/DeleteDenunciaDocumento", Method.GET);

            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("Id", Id);

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
        public RespuestaOutputDto DeleteByIdDenuncia(string url, string token, int Id)
        {
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaDocumento/DeleteDenunciaDocumentoByIdDenuncia", Method.GET);

            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("Id", Id);

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
