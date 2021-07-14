using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers._Common
{
    public class Pagination
    {
        public class Request
        {
            [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
            public int Page { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
            public int PageSize { get; set; }

            public string OrderBy { get; set; }
            public bool OrderDescending { get; set; }
        }

        public class Response<T>
        {
            public int Total { get; set; }
            public List<T> Items { get; set; }
        }

    }
}
