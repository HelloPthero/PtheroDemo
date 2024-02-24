using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PtheroDemo.Domain.Shared.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain.Shared.Base
{
    /// <summary>
    /// 友好异常过滤
    /// </summary>
    public class FriendlyExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if(filterContext.Exception is FriendlyException)
            {
                var exception = filterContext.Exception as FriendlyException;
                var dataResult = DataResult.Failure(exception.Message);
                filterContext.Result = new ObjectResult(dataResult)
                {
                    StatusCode = 500
                };
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
