namespace CanalDenunciaWeb.Data.Package.DenunciaPkg
{
    public class DenunciaInputDto
    {
        public int IdDenuncia { get; set; }
        public int IdDepartamentoSede { get; set; }
        public int IdTipoDelito { get; set; }
        public int IdEstadoDenuncia { get; set; }
        public string Denuncia { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Descripcion { get; set; }
        public int IdUsuario { get; set; }
        public int Activo { get; set; }
    }
}
