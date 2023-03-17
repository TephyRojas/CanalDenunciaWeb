using System.ComponentModel.DataAnnotations;

namespace CanalDenunciaWeb.Models
{
    public class vmDenunciaEstado
    {
        [Required]
        public int IdDenuncia { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int IdEstadoDenuncia { get; set; }
        [StringLength(500,ErrorMessage ="Excede el máximo permitido")]
        public string Comentario { get; set; }
    }
}