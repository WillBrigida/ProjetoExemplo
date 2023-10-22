namespace Core.Modules.Models
{
    public class GenericResponse<T> : GenericResponse
    {
        public T? Data { get; set; }
    }

    public class GenericResponse : ResponseBase { }


    public class LoginResponse<T> : GenericResponse<T>
    {
        public string? Token { get; set; }
    }

    public class ResponseBase
    {
        public bool Successful { get; set; }
        public int StatusCode { get; set; }
        public string Error { get; set; } = string.Empty;
        public string? Message { get; set; } = string.Empty;
    }

}
