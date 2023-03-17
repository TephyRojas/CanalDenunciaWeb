using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaCorreoAnonimoPkg
{
    public class DenunciaCorreoAnonimoInputDto
    {
        [Required]
        public int IdDenuncia { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
