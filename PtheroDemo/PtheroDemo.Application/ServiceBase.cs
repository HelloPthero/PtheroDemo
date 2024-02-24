using Microsoft.AspNetCore.Http;
using PtheroDemo.Application.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Application
{
    public class ServiceBase
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        //public CurrentUser CurrentUser { get; set; }

        public ServiceBase()  
        {
            //CurrentUser = HttpContextAccessor.HttpContext.Items["CurrentUser"] as CurrentUser;  
        }

        protected CurrentUser GetCurrentUser()
        {
            return HttpContextAccessor.HttpContext.Items["CurrentUser"] as CurrentUser;
        }
    }
}
