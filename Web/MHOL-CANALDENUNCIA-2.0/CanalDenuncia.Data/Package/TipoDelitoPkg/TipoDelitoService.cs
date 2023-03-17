using CanalDenunciaWeb.Data.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace CanalDenunciaWeb.Data.Package.TipoDelitoPkg
{
    public class TipoDelitoService
    {
        public RespuestaListOutputDto SelectListCombobox(string url, string token)
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("TipoDelito/TipoDelitoCombobox", Method.GET);
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

        public RespuestaByIdOutputDto SelectDescripcionByIdTipoDelito(string url, string token, string idTipoDelito)
        {
            var salida = new RespuestaByIdOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("TipoDelito/TipoDelitoDescripcionByIdTipoDelito", Method.GET);
            request.AddHeader("Authorization", token);
            request.AddParameter("idTipoDelito", idTipoDelito);
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

        public RespuestaByIdOutputDto SelectNombreByIdTipoDelito(string url, string token, string idTipoDelito)
        {
            var salida = new RespuestaByIdOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("TipoDelito/TipoDelitoNombreByIdTipoDelito", Method.GET);
            request.AddHeader("Authorization", token);
            request.AddParameter("idTipoDelito", idTipoDelito);
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

    }
}
