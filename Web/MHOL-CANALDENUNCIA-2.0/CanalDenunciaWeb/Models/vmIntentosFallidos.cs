using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CanalDenunciaWeb.Models
{
    public class vmIntentosFallidos
    {
        [Required]
        public string Usuario { get; set; }
        [MinLength(0)]
        public int Intentos { get; set; }
        [Required]
        public long IdUsuario { get; set; }
    }
}