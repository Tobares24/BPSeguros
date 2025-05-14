namespace Common.Services
{
    public class BPSegurosException : Exception
    {
        public int StatusCode { get; set; }
        public string? FieldName { get; set; }

        public BPSegurosException(int statusCode, string message, Exception? innerException = null, string? fieldName = null) : base(message, innerException)
        {
            StatusCode = statusCode;
            FieldName = fieldName;
        }
    }
}