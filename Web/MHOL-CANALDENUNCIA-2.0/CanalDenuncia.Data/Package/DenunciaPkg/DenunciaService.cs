
using CanalDenunciaWeb.Data.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace CanalDenunciaWeb.Data.Package.DenunciaPkg
{
    public class DenunciaService
    {
        public RespuestaListOutputDto SelectList(string url, string token, string Denuncia, string fechaInicio, string fechaFin, string inicioOcurrencia, string finOcurrencia, string IdSede, string IdDepartamento, string IdTipoDelito, string IdEstadoDenuncia, string idUsuario = "",string idRol="")
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("Denuncia/DenunciaList", Method.GET);
            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;

            if (Denuncia.Length > 0)
            {
                request.AddParameter("Denuncia", Denuncia);
            }

            if (IdSede.Length > 0)
            {
                if (IdSede != "0")
                {
                    request.AddParameter("idSede", IdSede);
                }
            }

            if (IdDepartamento.Length > 0)
            {
                if (IdDepartamento != "0")
                {
                    request.AddParameter("idDepartamento", IdDepartamento);
                }
            }

            if (fechaInicio.Length > 0)
            {
                request.AddParameter("fechaInicio", fechaInicio);
            }

            if (fechaFin.Length > 0)
            {
                request.AddParameter("fechaFin", fechaFin);
            }

            if (inicioOcurrencia.Length > 0)
            {
                request.AddParameter("inicioOcurrencia", inicioOcurrencia);
            }

            if (finOcurrencia.Length > 0)
            {
                request.AddParameter("finOcurrencia", finOcurrencia);
            }

            if (IdTipoDelito.Length > 0)
            {
                if (IdTipoDelito != "0")
                {
                    request.AddParameter("idTipoDelito", IdTipoDelito);
                }
            }

            if (IdEstadoDenuncia.Length > 0)
            {
                if (IdEstadoDenuncia != "0")
                {
                    request.AddParameter("idEstado", IdEstadoDenuncia);
                }
            }

            if (idUsuario.Length > 0)
            {
                if (idUsuario != "0")
                {
                    request.AddParameter("idUsuario", idUsuario);
                }
            }
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

        public RespuestaListOutputDto SelectReporteList(string url, string token, string Denuncia, string fechaInicio, string fechaFin, string inicioOcurrencia, string finOcurrencia, string IdSede, string IdDepartamento, string IdTipoDelito, string IdEstadoDenuncia, string idUsuario = "")
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("Denuncia/DenunciaReporteList", Method.GET);
            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;

            if (Denuncia.Length > 0)
            {
                request.AddParameter("Denuncia", Denuncia);
            }

            if (IdSede.Length > 0)
            {
                if (IdSede != "0")
                {
                    request.AddParameter("idSede", IdSede);
                }
            }

            if (IdDepartamento.Length > 0)
            {
                if (IdDepartamento != "0")
                {
                    request.AddParameter("idDepartamento", IdDepartamento);
                }
            }

            if (fechaInicio.Length > 0)
            {
                request.AddParameter("fechaInicio", fechaInicio);
            }

            if (fechaFin.Length > 0)
            {
                request.AddParameter("fechaFin", fechaFin);
            }

            if (inicioOcurrencia.Length > 0)
            {
                request.AddParameter("inicioOcurrencia", inicioOcurrencia);
            }

            if (finOcurrencia.Length > 0)
            {
                request.AddParameter("finOcurrencia", finOcurrencia);
            }

            if (IdTipoDelito.Length > 0)
            {
                if (IdTipoDelito != "0")
                {
                    request.AddParameter("idTipoDelito", IdTipoDelito);
                }
            }

            if (IdEstadoDenuncia.Length > 0)
            {
                if (IdEstadoDenuncia != "0")
                {
                    request.AddParameter("idEstadoDenuncia", IdEstadoDenuncia);
                }
            }

            if (idUsuario.Length > 0)
            {
                if (idUsuario != "0")
                {
                    request.AddParameter("idUsuario", idUsuario);
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

        public RespuestaByIdOutputDto SelectDenuncia(string url, string token, int IdDenuncia)
        {
            var salida = new RespuestaByIdOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("Denuncia/DenunciaDetalleByIdDenuncia", Method.GET);
            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("idDenuncia", IdDenuncia);


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

        public RespuestaByIdOutputDto SelectIdDenunciaByDenuncia(string url, string token, string Denuncia)
        {
            var salida = new RespuestaByIdOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("Denuncia/DenunciaDetalleByDenuncia", Method.GET);
            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("Denuncia", Denuncia);


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

        public RespuestaOutputDto Insert(string url, string token, DenunciaInputDto entity)
        {
            var client = new RestClient(url);
            var request = new RestRequest("Denuncia/PostDenuncia", Method.POST);

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

        public RespuestaOutputDto UpdateEstado(string url, string token, DenunciaEstadoInputDto entity)
        {
            var client = new RestClient(url);
            var request = new RestRequest("Denuncia/PutDenunciaEstado", Method.POST);

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
        public RespuestaOutputDto Update(string url, string token, DenunciaInputDto entity)
        {
            var client = new RestClient(url);
            var request = new RestRequest("Denuncia/PutDenuncia", Method.POST);

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
            var request = new RestRequest("Denuncia/DeleteDenuncia", Method.GET);

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
