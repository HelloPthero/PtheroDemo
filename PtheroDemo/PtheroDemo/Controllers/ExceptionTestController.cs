using Microsoft.AspNetCore.Mvc;
using PtheroDemo.Application.Contract.Dtos.Department;
using PtheroDemo.Application.Service;
using PtheroDemo.Domain.Shared.Base;
using PtheroDemo.Domain.Shared.BaseClass;

namespace PtheroDemo.Host.Controllers
{
    /// <summary>
    /// 异常测试
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class ExceptionTestController:ControllerBase
    {
        /// <summary>
        /// 正常异常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public void Exception()
        {
            throw new Exception("这是一个正常异常");
        }

        /// <summary>
        /// 友好异常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public void FriendlyException()
        {
            throw new FriendlyException("这是一个友好异常"); 
        }
    }
}
