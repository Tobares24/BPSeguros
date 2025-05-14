using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class ObtenerRequestModel
    {
        [Required(ErrorMessage = "La página actual es requerida.")]
        public int PaginaActual { get; set; }

        [Required(ErrorMessage = "La cantidad de registros es requerido.")]
        public int CantidadRegistros { get; set; }
    }
}