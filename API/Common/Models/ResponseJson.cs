using System.Net;

namespace API.Common.Models
{
    public class ResponseJson<T>
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; protected set; } = true;
        public T Data { get; set; }
        public string Message { get; set; }

        public ResponseJson() { }

        public ResponseJson(T data, string message = null, HttpStatusCode statusCode = HttpStatusCode.OK, bool isSuccess = true)
        {
            StatusCode = (int)statusCode;
            Data = data;
            Message = message;
            IsSuccess = isSuccess;
        }
    }
}
