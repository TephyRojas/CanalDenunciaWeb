using CanalDenunciaWeb.Data.Package.DenunciaInvolucradoPkg;
using System.Collections.Generic;

namespace CanalDenunciaWeb.Models
{
    public class vmDenunciaInvolucradoDto
    {
        public List<DenunciaInvolucradoListOutputDto> listInvolucrado = new List<DenunciaInvolucradoListOutputDto>();
        public List<DenunciaInvolucradoListOutputDto> listInvolucradoCE = new List<DenunciaInvolucradoListOutputDto>();
    }
}