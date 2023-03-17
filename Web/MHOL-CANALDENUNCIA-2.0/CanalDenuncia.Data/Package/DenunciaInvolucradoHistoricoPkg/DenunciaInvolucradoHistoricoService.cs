using CanalDenunciaWeb.Data.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace CanalDenunciaWeb.Data.Package.DenunciaInvolucradoHistoricoPkg
{
    public class DenunciaInvolucradoHistoricoService
    {
        public RespuestaOutputDto Insert(string url, string token, DenunciaInvolucradoHistoricoInputDto entity)
        {
            var salida = new RespuestaOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaInvolucradoHistorico/PostDenunciaInvolucradoHistorico", Method.POST);

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

            salida = JsonConvert.DeserializeObject<RespuestaOutputDto>(response.Content.ToString());

            return salida;
        }
    }
}
