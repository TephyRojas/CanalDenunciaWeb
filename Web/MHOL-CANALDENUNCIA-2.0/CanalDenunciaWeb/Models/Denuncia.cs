using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace CanalDenunciaWeb.Models
{
    public class Denuncia
    {
        [Required(ErrorMessage = "Id Denuncia obligatorio")]
        public int idDenuncia { get; set; }
        public int Usuario { get; set; }
        public string Identificador { get; set; }
        public int PermiteModificacion { get; set; }
        public int IdEstadoDenuncia { get; set; }
        public string directorio { get; set; }
        public List<Involucrados> involucrados { get; set; }
        public List<Documentos> documentos { get; set; }
        public Detalle detalles { get; set; }
        public List<Comentarios> comentarios { get; set; }
    }
    public class Involucrados
    {
        [Required(ErrorMessage = "Usuario es obligatorio")]
        public int idUsuario { get; set; }
        [Required(ErrorMessage = "Nombre es obligatorio")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Sede es obligatorio")]
        public string sede { get; set; }
        [Required(ErrorMessage = "Departamento es obligatorio")]
        public string departamento { get; set; }
        public int tipo { get; set; } //Tipo 0 Comité
    }

    public class Documentos
    {
        public string nombre { get; set; }
        public string extension { get; set; }
        public string ruta { get; set; }
        public HttpPostedFileBase archivo { get; set; }
    }

    public class Detalle
    {
        [Required(ErrorMessage = "Fecha Inicio es obligatorio")]
        public string FechaInicio { get; set; }
        [Required(ErrorMessage = "Fecha Fin es obligatorio")]
        public string FechaFin { get; set; }
        [Required(ErrorMessage = "Tipo Delito es obligatorio")]
        public int IdTipoDelito { get; set; }
        [StringLength(3000, ErrorMessage = "Descripción permite un máximo de {1} caracteres ")]
        public string Comentario { get; set; }
        public string Descripcion { get; set; }
        public int IdDepartamentoSede { get; set; }
        public int IdSede { get; set; }
    }

    public class Comentarios
    {
        public int IdDenunciaComentario { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Comentario { get; set; }
        public string Fecha { get; set; }
    }
}

