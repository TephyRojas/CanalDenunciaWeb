namespace CanalDenunciaWeb.Data.Package.DenunciaInvolucradoHistoricoPkg
{
    public class DenunciaInvolucradoHistoricoInputDto
    {
        public int IdDenunciaInvolucrado { get; set; }
        public int idDenuncia { get; set; }
        public int idUsuario { get; set; }
        public string FechaModificacion { get; set; }
    }
}
