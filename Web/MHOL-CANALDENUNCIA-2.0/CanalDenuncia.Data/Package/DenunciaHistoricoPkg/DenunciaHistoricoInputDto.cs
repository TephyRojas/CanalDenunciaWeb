namespace CanalDenunciaWeb.Data.Package.DenunciaHistoricoPkg
{
    public class DenunciaHistoricoInputDto
    {
        public int IdDenunciaHistorico { get; set; }
        public int IdDenuncia { get; set; }
        public int IdDepartamentoSede { get; set; }
        public int IdTipoDelito { get; set; }
        public int IdEstadoDenuncia { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public int IdUsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }
    }
}
