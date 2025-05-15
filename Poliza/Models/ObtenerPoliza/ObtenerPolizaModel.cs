namespace Poliza.Models.ObtenerPoliza
{
    public class ObtenerPolizaModel
    {
        public Guid Id { get; set; }
        public string? NumeroPoliza { get; set; }
        public string? TipoPoliza { get; set; }
        public string? CedulaAsegurado { get; set; }
        public string? NombreCompleto { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string? PolizaEstado { get; set; }
    }
}
