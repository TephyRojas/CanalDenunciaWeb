namespace CanalDenunciaWeb.Models
{
    public class SesionDenunciaDto
    {
        public int idDenuncia { get; set; }
        public class DatosInvolucrados
        {
            public int idUsuario { get; set; }
        }

        public class DenunciaDocumento
        {
            public string nombre { get; set; }
            public string extension { get; set; }
            public string archivo { get; set; }
        }
    }
}