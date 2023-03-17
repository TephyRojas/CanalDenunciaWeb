using CanalDenunciaWeb.Data.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace CanalDenunciaWeb.Data.Package.DepartamentoSedePkg
{
    public class DepartamentoSedeService
    {
        public RespuestaListOutputDto SelectListCombobox(string url, string token, string idSede)
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("DepartamentoSede/DepartamentoSedeCombobox", Method.GET);
            request.AddHeader("Authorization", token);
            request.AddParameter("idSede", idSede);
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

        public RespuestaOutputDto SelectExist(string url, string token, string Sede, string Departamento)
        {
            var salida = new RespuestaOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("DepartamentoSede/DepartamentoSedeExist", Method.GET);

            request.AddHeader("Authorization", token);
            request.AddParameter("departamento", Departamento);
            request.AddParameter("sede", Sede);
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
