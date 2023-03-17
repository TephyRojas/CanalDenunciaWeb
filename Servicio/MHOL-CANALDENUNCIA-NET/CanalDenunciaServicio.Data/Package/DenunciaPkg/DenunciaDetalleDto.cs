using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanalDenunciaServicio.Data.Package.DenunciaPkg
{
    public class DenunciaDetalleDto
    {
        public string Denuncia { get; set; }
        public int IdDepartamentoSede { get; set; }
        public int IdTipoDelito { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Descripcion { get; set; }
        public int IdEstadoDenuncia { get; set; }
        public int IdSede { get; set; }
        public int PermiteModificacion { get; set; }
        public List<DenunciaInvolucrado> Involucrados { get; set; }
        public List<DenunciaDocumento> Documentos { get; set; }
        public List<DenunciaComentario> Comentarios { get; set; }
    }

    public class DenunciaInvolucrado
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public int IdRol { get; set; }
        public int IdSede { get; set; }
        public string Departamento { get; set; }
        public string Sede { get; set; }
    }

    public class DenunciaDocumento
    {
        public int IdDenunciaDocumento { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public byte[] Archivo { get; set; }
    }

    public class DenunciaComentario
    {
        public int IdDenunciaComentario { get; set; }
        public string Usuario { get; set; }
        public string Comentario { get; set; }
        public string Fecha { get; set; }
    }
       
}
