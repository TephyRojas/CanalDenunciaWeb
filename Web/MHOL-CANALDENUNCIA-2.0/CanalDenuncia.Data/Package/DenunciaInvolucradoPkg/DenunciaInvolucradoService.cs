
using CanalDenunciaWeb.Data.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace CanalDenunciaWeb.Data.Package.DenunciaInvolucradoPkg
{
    public class DenunciaInvolucradoService
    {

        public RespuestaListOutputDto SelectListSeleccionado(string url, string token, string idDenuncia, string rol, string nombre = "", string idSede = "", string idDepartamento = "")
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaInvolucrado/DenunciaInvolucradoListSeleccionado", Method.GET);
            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;

            if (idDenuncia.Length > 0)
            {
                request.AddParameter("idDenuncia", idDenuncia);
            }

            if (rol.Length > 0)
            {
                if (rol != "0")
                {
                    request.AddParameter("rol", rol);
                }
            }

            if (nombre.Length > 0)
            {

                request.AddParameter("nombre", nombre);

            }

            if (idSede.Length > 0)
            {
                if (idSede != "0")
                {
                    request.AddParameter("idSede", idSede);
                }
            }

            if (idDepartamento.Length > 0)
            {
                if (idDepartamento != "0")
                {
                    request.AddParameter("idDepartamento", idDepartamento);
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

        public RespuestaListOutputDto SelectListNoSeleccionado(string url, string token, string idDenuncia, string rol, string nombre = "", string idSede = "", string idDepartamento = "")
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaInvolucrado/DenunciaInvolucradoListNoSeleccionado", Method.GET);
            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;

            if (idDenuncia.Length > 0)
            {
                request.AddParameter("idDenuncia", idDenuncia);
            }

            if (rol.Length > 0)
            {
                if (rol != "0")
                {
                    request.AddParameter("rol", rol);
                }
            }

            if (nombre.Length > 0)
            {

                request.AddParameter("nombre", nombre);

            }

            if (idSede.Length > 0)
            {
                if (idSede != "0")
                {
                    request.AddParameter("idSede", idSede);
                }
            }

            if (idDepartamento.Length > 0)
            {
                if (idDepartamento != "0")
                {
                    request.AddParameter("idDepartamento", idDepartamento);
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

        public RespuestaListOutputDto SelectListByIdRol(string url, string token, string rol, string nombre = "", string idSede = "", string idDepartamento = "", string rolExcluido = "")
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaInvolucrado/DenunciaInvolucradoByIdRol", Method.GET);
            request.AddHeader("Authorization", token);
            request.RequestFormat = DataFormat.Json;


            if (rol.Length > 0)
            {
                if (rol != "0")
                {
                    request.AddParameter("rol", rol);
                }
            }

            if (nombre.Length > 0)
            {

                request.AddParameter("nombre", nombre);

            }

            if (idSede.Length > 0)
            {
                if (idSede != "0")
                {
                    request.AddParameter("idSede", idSede);
                }
            }

            if (idDepartamento.Length > 0)
            {
                if (idDepartamento != "0")
                {
                    request.AddParameter("idDepartamento", idDepartamento);
                }
            }
            if (rolExcluido != "")
            {
                request.AddParameter("rolExcluido", rolExcluido);
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

        public RespuestaOutputDto Insert(string url, string token, DenunciaInvolucradoInputDto entity)
        {
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaInvolucrado/PostDenunciaInvolucrado", Method.POST);

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

        public RespuestaOutputDto Update(string url, string token, DenunciaInvolucradoInputDto entity)
        {
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaInvolucrado/PutDenunciaInvolucrado", Method.POST);

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
            var request = new RestRequest("DenunciaInvolucrado/DeleteDenunciaInvolucrado", Method.GET);

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
            var request = new RestRequest("DenunciaInvolucrado/DeleteDenunciaInvolucradoByIdDenuncia", Method.GET);

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

        public RespuestaListOutputDto SelectByIdDenuncia(string url, string token, string Denuncia)
        {
            var salida = new RespuestaListOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaInvolucrado/DenunciaInvolucradoByIdDenuncia", Method.GET);
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

        public RespuestaOutputDto SelectInvolucradoDenuncia(string url, string token, int idDenuncia, int idUsuario)
        {
            var salida = new RespuestaOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaInvolucrado/DenunciaInvolucradoByIdUserId", Method.GET);

            request.AddHeader("Authorization", token);
            request.AddParameter("id", idDenuncia);
            request.AddParameter("idUsuario", idUsuario);
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

        public RespuestaOutputDto SelectInvolucradoRolDenuncia(string url, string token, int idDenuncia)
        {
            var salida = new RespuestaOutputDto();
            var client = new RestClient(url);
            var request = new RestRequest("DenunciaInvolucrado/DenunciaInvolucradoByIdRol", Method.GET);

            request.AddHeader("Authorization", token);
            request.AddParameter("id", idDenuncia);
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
