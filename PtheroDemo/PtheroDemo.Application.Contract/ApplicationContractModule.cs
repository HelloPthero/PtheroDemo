using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Application.Contract
{
    public class ApplicationContractModule : Domain.Shared.Base.IModule
    {
        public void ConfigureServices(ContainerBuilder containerBuilder, IServiceCollection services, IConfiguration configuration)
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(ApplicationContractModule).Assembly); // 添加 AutoMapper 映射配置类所在的程序集
            });

            containerBuilder.RegisterInstance(mapperConfiguration.CreateMapper()).As<IMapper>().SingleInstance();
        }
    }
}
