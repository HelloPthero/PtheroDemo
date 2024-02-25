using Autofac;
using Autofac.Core;
using Autofac.Features.AttributeFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PtheroDemo.Application.Base;
using PtheroDemo.Application.Contract.IService;
using PtheroDemo.Application.Service;
using PtheroDemo.Domain.Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Hangfire;
using Hangfire.EntityFrameworkCore;
using Hangfire.SqlServer;
using Autofac.Extensions.DependencyInjection;
using PtheroDemo.EntityFrameworkCore;

namespace PtheroDemo.Application
{
    public class ApplicationModule : Domain.Shared.Base.IModule
    {
        public void ConfigureServices(ContainerBuilder containerBuilder, IServiceCollection services,IConfiguration configuration)
        {
            #region currentUser
            containerBuilder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance(); //当前用户注入

            containerBuilder.RegisterType<ServiceBase>().PropertiesAutowired();

            #endregion

            #region redis分布式缓存

            // 注册Redis连接实例
            containerBuilder.Register(c =>
            {
                var cfg = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(cfg); 
            })
            .As<ConnectionMultiplexer>()
            .SingleInstance();

            // 注册IDistributedCache服务
            containerBuilder.Register(c =>
            {
                var redisConnection = c.Resolve<ConnectionMultiplexer>();
                return new RedisCache(new RedisCacheOptions
                {
                    Configuration = redisConnection.Configuration
                });
            })
            .As<IDistributedCache>()
            .SingleInstance();

            #endregion

            #region 应用服务

            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            containerBuilder.RegisterAssemblyTypes(currentAssembly)  //服务层注入
                .Where(t => t.Name.EndsWith("Service"))  //当一个接口有多个调用时，用于过滤
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired();

            #endregion


            #region hangfire

            //services.AddHangfire(cf => cf
            //    .UseSqlServerStorage(configuration.GetConnectionString("default")));

            var hfContainerBuilder = new ContainerBuilder();
            hfContainerBuilder.Populate(services);
            var container = hfContainerBuilder.Build();
            // 将 Autofac 容器设置为 Hangfire 的作业解析器
            GlobalConfiguration.Configuration.UseAutofacActivator(container);
            #endregion
        }

    }
}
