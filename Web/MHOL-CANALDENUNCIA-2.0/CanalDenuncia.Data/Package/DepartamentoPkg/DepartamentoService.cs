using CanalDenunciaWeb.Data.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace CanalDenunciaWeb.Data.Package.DepartamentoPkg
{
    public class DepartamentoService
    {
        public RespuestaListOutputDto SelectListCombobox(string url, string token, int idSede)
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("Departamento/DepartamentoCombobox", Method.GET);
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
    }
}
