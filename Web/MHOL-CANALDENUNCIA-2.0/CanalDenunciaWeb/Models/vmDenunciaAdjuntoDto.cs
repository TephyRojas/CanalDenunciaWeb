using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CanalDenunciaWeb.Models
{
    public class vmDenunciaAdjuntoDto
    {
        [Required(ErrorMessage = "Número de denuncia Obligatorio")]
        public int idDenuncia { get; set; }
        public List<string> adjuntos { get; set; }

    }
}