using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PtheroDemo.Domain.Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain
{
    public class DomainModule : IModule
    {
        public void ConfigureServices(ContainerBuilder containerBuilder, IServiceCollection services, IConfiguration configuration)
        {
            //containerBuilder.Register(typeof())
            // 注册泛型 Repository
            containerBuilder.RegisterGeneric(typeof(Repository<,>))
                   .As(typeof(IRepository<,>))
                   .InstancePerLifetimeScope()
                   .PropertiesAutowired(new CustomPropertySelector())
                   ; // 根据你的需求设置生命周期
        }
    }
}
