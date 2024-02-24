using PtheroDemo.Domain.Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Application.Base
{
    /// <summary>
    /// 当前用户
    /// </summary>
    public class CurrentUser: ICurrentUser
    {
        public int UserId { get; set; } 

        public string UserName { get; set; }
    }
}
