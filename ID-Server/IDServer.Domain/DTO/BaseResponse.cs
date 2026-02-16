using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IDServer.Domain.DTO
{
    public class BaseResponse<T> where T : class
    {

        public int Code { get; set; } 
        public string Message { get; set; }
        public Pagination? Pagination { get; set; }
        public Object? Data { get; set; }
       

        public BaseResponse(int code, string message, Pagination pagination, List<T>? data)
        {
            Code = code;
            Message = message;
            Pagination = pagination;
            Data = data;
        }

        public BaseResponse(int code, string message, Pagination pagination, T? data)
        {
            Code = code;
            Message = message;
            Pagination = pagination;
            Data = data;
        }

        public BaseResponse(int code, string message, int? data)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        public BaseResponse(int code, string message)
        {
            Code = code;
            Message = message;
            Pagination = null;
            Data = null;
        }
    }

    public class Pagination
    {
        [JsonIgnore]
        public int PageIndex { get; set; }

        [JsonIgnore]
        public int PageSize { get; set; }
        public int recordCount { get; set; }

        public Pagination(int pageIndex, int pageSize, int RecordCount)
        {
            PageIndex = pageIndex;
            if (pageSize > 0)
            {
                PageSize = pageSize;
            }
            else
            {
                PageSize = 1;
            }
            recordCount = RecordCount;
        }

        public int pageCount
        {
            get { return (int)Math.Ceiling((double)recordCount / PageSize); }
        }
    }
}

