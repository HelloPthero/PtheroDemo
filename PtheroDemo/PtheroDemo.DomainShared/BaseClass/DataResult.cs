using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain.Shared.BaseClass
{
    public class DataResult
    {
        public bool IsSuccess { get; set; }
         
        public string Message { get; set; }

        public Object Data { get; set; }

        private DataResult(bool isSuccess, string message, Object data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static DataResult Success(string message = "",Object data = null)
        {
            return new DataResult(true, message, data);
        }

        public static DataResult Failure(string message)
        {
            return new DataResult(false, message, default);
        }

    }

}
