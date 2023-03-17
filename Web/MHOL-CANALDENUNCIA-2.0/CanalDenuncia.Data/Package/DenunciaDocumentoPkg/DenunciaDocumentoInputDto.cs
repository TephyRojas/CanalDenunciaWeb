namespace CanalDenunciaWeb.Data.Package.DenunciaDocumentoPkg
{
    public class DenunciaDocumentoInputDto
    {
        public int IdDenuncia { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public string Ruta { get; set; }
        public int Activo { get; set; }
    }
}
