using System.ComponentModel.DataAnnotations;

namespace Poliza.Models.ActualizarPoliza
{
    public class ActualizarPolizaRequestModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número de póliza es requerido.")]
        [RegularExpression(@"^[A-Za-z0-9\-]{1,128}$", ErrorMessage = "El número de póliza puede tener letras, números o guiones y no exceder los 64 caracteres.")]
        public string? NumeroPoliza { get; set; }

        [Required(ErrorMessage = "El identificador del tipo de póliza es obligatorio.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}[}]?$", ErrorMessage = "El identificador de tipo de poliza debe tener un formato GUID válido.")]
        public string? IdTipoPoliza { get; set; }

        public decimal? MontoAsegurado { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es requerida.")]
        public DateTime FechaVencimiento { get; set; }

        public DateTime? FechaEmision { get; set; }

        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}[}]?$", ErrorMessage = "El identificador de la cobertura debe tener un formato GUID válido.")]
        public string? IdCobertura { get; set; }

        [Required(ErrorMessage = "El estdo es requerido.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}[}]?$", ErrorMessage = "El identificador del estado debe tener un formato GUID válido.")]
        public string? IdPolizaEstado { get; set; }

        public decimal? Prima { get; set; }

        public DateTime? Periodo { get; set; }

        public DateTime? FechaInclusion { get; set; }

        [RegularExpression(@"^[A-Za-z0-9]{1,128}$", ErrorMessage = "La aseguradora puede tener letras, números y no exceder los 128 caracteres.")]
        public string? Aseguradora { get; set; }
    }
}
