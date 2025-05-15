namespace Poliza.Models.ObtenerPorId
{
    public class ObtenerPorIdResponseModel
    {
        public string? Id { get; set; }
        public string? NumeroPoliza { get; set; }
        public string? IdTipoPoliza { get; set; }
        public string? CedulaAsegurado { get; set; }
        public decimal? MontoAsegurado { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime? FechaEmision { get; set; }
        public string? IdCobertura { get; set; }
        public string? IdPolizaEstado { get; set; }
        public decimal? Prima { get; set; }
        public DateTime? Periodo { get; set; }
        public DateTime? FechaInclusion { get; set; }
        public string? Aseguradora { get; set; }
    }
}
