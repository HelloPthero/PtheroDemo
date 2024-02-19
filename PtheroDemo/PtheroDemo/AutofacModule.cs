using Autofac;
using Autofac.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace PtheroDemo.Host
{
    public class AutofacModule : Autofac.Module
    {
        public IServiceCollection services { get; set; }

        public IConfiguration configuration { get; set; }
        public AutofacModule(IServiceCollection _services, IConfiguration _configuration)
        {
            services = _services;
            configuration = _configuration;
        }
        //重写load方法
        protected override void Load(ContainerBuilder containerBuilder)
        {
            //Assembly service = Assembly.Load("PtheroDemo.Application");
            //Assembly currentAssembly = Assembly.GetExecutingAssembly();
            //containerBuilder.RegisterAssemblyTypes(service)  //服务层注入
            //    .Where(t => t.Name.EndsWith("Service"))  //当一个接口有多个调用时，用于过滤
            //    .AsImplementedInterfaces()
            //    .InstancePerDependency()
            //    .PropertiesAutowired(new CustomPropertySelector()); ;

            //containerBuilder.RegisterType<CustomPropertySelector>().As<IPropertySelector>();
            //containerBuilder.RegisterApiControllers(typeof(Program).Assembly);
            //containerBuilder.RegisterControllers(typeof(Program).Assembly);



           // 获取所有符合约定的程序集
           var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"PtheroDemo.*.dll")
               .Select(Assembly.LoadFrom)
               .ToList();



            //获取所有模块的 IModule 实现并调用 ConfigureServices
            foreach (var assembly in assemblies)
            {
                var moduleTypes = assembly.GetTypes()
                    .Where(type => typeof(Domain.Shared.Base.IModule).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    .ToList();

                foreach (var moduleType in moduleTypes)
                {
                    var moduleInstance = Activator.CreateInstance(moduleType) as Domain.Shared.Base.IModule;
                    moduleInstance?.ConfigureServices(containerBuilder, services, configuration);
                }
            }
            Assembly service = Assembly.Load("PtheroDemo.Application");
            //Assembly currentAssembly = Assembly.GetExecutingAssembly();
            //containerBuilder.RegisterAssemblyTypes(service)  //服务层注入
            //    .Where(t => t.Name.EndsWith("Service"))  //当一个接口有多个调用时，用于过滤
            //    .AsImplementedInterfaces()
            //    .InstancePerDependency()
            //    .PropertiesAutowired(); ;

            //var serviceTypes = Assembly.GetExecutingAssembly()
            //                       .GetTypes()
            //                       .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any() && t.Name.EndsWith("Service"))
            //                       .ToList();

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

            var controllerBaseType = typeof(ControllerBase);   //控制器注入
            containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();

            //var applicationModule = new ApplicationModule();
            //applicationModule.ConfigureServices(containerBuilder, builder.Configuration);

            // 启用属性注入
            //containerBuilder.PropertiesAutowired();


        }
    }
}
