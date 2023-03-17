using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CanalDenunciaWeb.Models
{
    public class vmInvolucradoInputDto
    {
       [Required]
        public int IdDenuncia { get; set; }
        [Required]
        public List<Involucrado> Involucrados { get; set; }

        public class Involucrado
        {
            [Required]
            public int IdUsuario { get; set; }
            [MaxLength(64,ErrorMessage ="El campo es demasiado largo")]
            public string Nombre { get; set; }
            [MaxLength(32, ErrorMessage = "El campo es demasiado largo")]
            public string Sede { get; set; }
            [MaxLength(32, ErrorMessage = "El campo es demasiado largo")]
            public string Departamento { get; set; }
        }
    }
}