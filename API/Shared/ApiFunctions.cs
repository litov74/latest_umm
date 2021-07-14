using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared
{
    public enum ApiErrorCode
    {
        Validation = 1,
        Error = 2
    }
    public class ApiFunctions
    {
        public static ApiResponse<T> ApiSuccessResponse<T>(T responseData, string ErrorTitle = "")
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                ResponseData = responseData,
                ErrorTitle = ErrorTitle,
            };
        }

        public static ApiResponse<T> ApiValidationResponse<T>(string validationMessage, string ErrorTitle = "")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                ErrorCode = ApiErrorCode.Validation,
                ErrorMessage = validationMessage,
                ErrorTitle = ErrorTitle,
            };
        }

        public static ApiResponse<T> ApiErrorResponse<T>(string errorMessage, string ErrorTitle = "")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                ErrorCode = ApiErrorCode.Error,
                ErrorMessage = errorMessage,
                ErrorTitle = ErrorTitle,
            };
        }

        public static ApiResponse<T> ApiErrorResponse<T>(Exception ex, string ErrorTitle = "")
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                ErrorCode = ApiErrorCode.Error,
                ErrorMessage = ex.Message.ToString(),
                ErrorTitle = ErrorTitle,
            };
        }
        public class QueryResponse<T>
        {
            public List<T> Data { get; set; }
            public int RecordsTotal { get; set; }
            public int RecordsFiltered { get; set; }
        }
    }
}
