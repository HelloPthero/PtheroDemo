using Autofac;
using Autofac.Core;
using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PtheroDemo.Application.Contract.IService;
using PtheroDemo.Application.Service;
using PtheroDemo.Domain.Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Application
{
    public class ApplicationModule : Domain.Shared.Base.IModule
    {
        public void ConfigureServices(ContainerBuilder containerBuilder, IServiceCollection services,IConfiguration configuration)
        {
            // 使用反射扫描当前程序集中的所有服务
            //var serviceTypes = Assembly.GetExecutingAssembly()
            //                           .GetTypes()
            //                           .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any() && t.Name.EndsWith("Service"))
            //                           .ToList();

            //// 注册这些服务 
            //foreach (var serviceType in serviceTypes)
            //{
            //    var interfaces = serviceType.GetInterfaces();
            //    foreach (var @interface in interfaces)
            //    {
            //        containerBuilder.RegisterType(serviceType).As(@interface).WithAttributeFiltering().InstancePerLifetimeScope();
            //        //services.AddTransient(@interface, serviceType);
            //    }
            //}
            //Assembly service = Assembly.Load("PtheroDemo.Application");
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            containerBuilder.RegisterAssemblyTypes(currentAssembly)  //服务层注入
                .Where(t => t.Name.EndsWith("Service"))  //当一个接口有多个调用时，用于过滤
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired(); 

            //containerBuilder.RegisterType(typeof(UserService)).As(typeof(IUserService)).PropertiesAutowired();
        }
    }
}
