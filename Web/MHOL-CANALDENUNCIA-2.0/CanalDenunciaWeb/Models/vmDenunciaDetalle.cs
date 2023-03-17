using System;
using System.ComponentModel.DataAnnotations;

namespace CanalDenunciaWeb.Models
{
    public class vmDenunciaDetalle
    {
        [Required]
        public int IdDenuncia { get; set; }
        [Required(ErrorMessage = "Fecha de Ocurrencia es obligatorio")]
        public string Fecha { get; set; }
        [Required]
        [Range(1, Int64.MaxValue, ErrorMessage = "El campo Tipo Delito es obligatorio")]
        public int IdTipoDelito { get; set; }
        [StringLength(500,ErrorMessage ="Excede el máximo")]
        public string Descripcion { get; set; }
        [Range(0, Int64.MaxValue,ErrorMessage =" no puede ser un valor negativo")]
        public int IdDepartamentoSede { get; set; }
    }
}