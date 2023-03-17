namespace CanalDenunciaWeb.Models
{
    public class SesionUsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public string Rut { get; set; }
        public string Matriz { get; set; }
        public string Sucursal { get; set; }
        public string Email { get; set; }
        public int IdRol { get; set; }
        public string Token { get; set; }
        public string CorreoAnonimo { get; set; }
    }
}