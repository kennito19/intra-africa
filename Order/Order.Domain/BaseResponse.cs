using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain
{
    public class BaseResponse<T>
    {
        public int? code { get; set; }
        //public object? extension { get; set; }
        public string? message { get; set; }
        public object? data { get; set; }
    }
}
