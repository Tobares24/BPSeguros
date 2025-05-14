namespace Common.Models
{
    public class ErrorResponseModel
    {
        public int? StatusCode { get; set; }
        public string? TraceId { get; set; }
        public string? Message { get; set; }
        public string? FieldName { get; set; }
    }
}