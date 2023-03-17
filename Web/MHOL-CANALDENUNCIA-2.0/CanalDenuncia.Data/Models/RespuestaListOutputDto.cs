using System.Collections.Generic;

namespace CanalDenunciaWeb.Data.Models
{
    public class RespuestaListOutputDto
    {
        public respuesta Respuesta { get; set; }
        public resultado Resultado { get; set; }

        public class respuesta
        {
            public int CodigoRetorno { get; set; }
            public string Mensaje { get; set; }
            public string Token { get; set; }
        }

        public class resultado
        {
            public List<object> Data { get; set; }
        }
    }
}