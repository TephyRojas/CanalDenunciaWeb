namespace CanalDenunciaServicio.Models
{
    public class Token
    {
        public string exp { get; set; }
        public string nbf { get; set; }
        public string iat { get; set; }
        public string iss { get; set; }
        public string usuario { get; set; }
        public string email { get; set; }
        public string nombreUsuario { get; set; }
    }
}