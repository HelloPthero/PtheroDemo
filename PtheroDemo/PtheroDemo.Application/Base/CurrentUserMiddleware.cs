using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Application.Base
{
    /// <summary>
    /// 当前用户中间件
    /// </summary>
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = context.User.Identity.Name;
            var currentUser = new CurrentUser
            {
                
                UserId = userId == null?0: int.Parse(userId),
                UserName = userName
            };

            // 将当前用户信息存储在 HTTP 上下文中
            context.Items["CurrentUser"] = currentUser;

            await _next(context);
        }
    }
}
