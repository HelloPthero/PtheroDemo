using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.AttributeFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PtheroDemo.Application;
using PtheroDemo.Application.Contract.IService;
using PtheroDemo.Application.Service;
using PtheroDemo.Domain;
using PtheroDemo.Domain.Entities;
using PtheroDemo.Domain.Shared.Base;
using PtheroDemo.EntityFrameworkCore;
using System.Reflection;
using IModule = PtheroDemo.Domain.Shared.Base.IModule;

namespace PtheroDemo.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            builder.Services.AddControllers();


            //builder.Services.AddDbContext<DBContext>(db=>db.UseSqlServer(builder.Configuration.GetConnectionString("default"), b => b.MigrationsAssembly("PtheroDemo.EntityFrameworkCore")));

            //builder.Services.AddScoped(typeof(IDBContext), typeof(DBContext));

            //builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            LoadModulesWithPrefix(builder.Host, builder.Services,builder.Configuration);
            //var serviceProvider = services.BuildServiceProvider();
            //builder.Services.AddScoped(typeof(IUserService), typeof(UserService));

            // 获取所有实现了 IModule 接口的类型
            //var moduleTypes = Assembly.GetExecutingAssembly()
            //        .GetTypes()
            //        .Where(type => typeof(IModule).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            //        .ToList();

            // 遍历并实例化这些模块，并调用其 ConfigureServices 方法
            //
            //foreach (var moduleType in moduleTypes)
            //{
            //    var moduleInstance = Activator.CreateInstance(moduleType) as IModule;
            //    moduleInstance?.ConfigureServices(services);
            //}

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void LoadModulesWithPrefix(ConfigureHostBuilder hostBuilder, IServiceCollection services,IConfiguration configuration)
        {

            
            hostBuilder.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                var auModule = new AutofacModule(services, configuration);
                containerBuilder.RegisterModule(auModule);
                //containerBuilder.RegisterModule<AutofacModule>();
                //Assembly service = Assembly.Load("PtheroDemo.Application");
                //Assembly currentAssembly = Assembly.GetExecutingAssembly();
                //containerBuilder.RegisterAssemblyTypes(service)  //服务层注入
                //    .Where(t => t.Name.EndsWith("Service"))  //当一个接口有多个调用时，用于过滤
                //    .AsImplementedInterfaces()
                //    .InstancePerDependency()
                //    .PropertiesAutowired(new CustomPropertySelector()); ;

                //containerBuilder.RegisterType<CustomPropertySelector>().As<IPropertySelector>();
                ////containerBuilder.RegisterApiControllers(typeof(Program).Assembly);
                ////containerBuilder.RegisterControllers(typeof(Program).Assembly);



                //// 获取所有符合约定的程序集
                //var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"PtheroDemo.*.dll") 
                //    .Select(Assembly.LoadFrom)
                //    .ToList();



                //// 获取所有模块的 IModule 实现并调用 ConfigureServices
                //foreach (var assembly in assemblies)
                //{
                //    var moduleTypes = assembly.GetTypes()
                //        .Where(type => typeof(IModule).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                //        .ToList();

                //    foreach (var moduleType in moduleTypes)
                //    {
                //        var moduleInstance = Activator.CreateInstance(moduleType) as IModule;
                //        moduleInstance?.ConfigureServices(containerBuilder, services, configuration);
                //    }
                //}

                ////var serviceTypes = Assembly.GetExecutingAssembly()
                ////                       .GetTypes()
                ////                       .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any() && t.Name.EndsWith("Service"))
                ////                       .ToList();

                ////// 注册这些服务 
                ////foreach (var serviceType in serviceTypes)
                ////{
                ////    var interfaces = serviceType.GetInterfaces();
                ////    foreach (var @interface in interfaces)
                ////    {
                ////        containerBuilder.RegisterType(serviceType).As(@interface).WithAttributeFiltering().InstancePerLifetimeScope();
                ////        //services.AddTransient(@interface, serviceType);
                ////    }
                ////}

                //var controllerBaseType = typeof(ControllerBase);   //控制器注入
                //containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
                //    .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                //    .PropertiesAutowired(new CustomPropertySelector());

                ////var applicationModule = new ApplicationModule();
                ////applicationModule.ConfigureServices(containerBuilder, builder.Configuration);

                //// 启用属性注入
                ////containerBuilder.PropertiesAutowired();
            });
        }
    }
}