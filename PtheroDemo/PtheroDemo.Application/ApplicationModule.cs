using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PtheroDemo.Domain.Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Application
{
    public class ApplicationModule : IModule
    {
        public void ConfigureServices(IServiceCollection services,IConfiguration configuration)
        {
            // 使用反射扫描当前程序集中的所有服务
            var serviceTypes = Assembly.GetExecutingAssembly()
                                       .GetTypes()
                                       .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any() && t.Name.EndsWith("Service"))
                                       .ToList();

            // 注册这些服务
            foreach (var serviceType in serviceTypes)
            {
                var interfaces = serviceType.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    services.AddTransient(@interface, serviceType);
                }
            }
        }
    }
}
