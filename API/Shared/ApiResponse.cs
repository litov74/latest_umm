using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T ResponseData { get; set; }
        public ApiErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorTitle { get; set; }
    }
}
