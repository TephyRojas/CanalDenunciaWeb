namespace CanalDenunciaWeb.Data.Package.DenunciaPkg
{
    public class DenunciaOutputDto
    {
        public int IdDenuncia { get; set; }
        public int IdDepartamentoSede { get; set; }
        public int IdTipoDelito { get; set; }
        public int IdEstadoDenuncia { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }

    }
}
