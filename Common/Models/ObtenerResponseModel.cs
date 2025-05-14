namespace Common.Models
{
    public class ObtenerResponseModel<T>
    {
        public int PaginaActual { get; set; }
        public int CantidadRegistros { get; set; }
        public int CantidadPaginas { get; set; }
        public int CantidadRegistrosPaginas { get; set; }
        public List<T>? Registros { get; set; }
    }
}