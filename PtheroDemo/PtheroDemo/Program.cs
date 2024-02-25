using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.AttributeFilters;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PtheroDemo.Application;
using PtheroDemo.Application.Base;
using PtheroDemo.Application.Contract.IService;
using PtheroDemo.Application.Service;
using PtheroDemo.Domain;
using PtheroDemo.Domain.Entities;
using PtheroDemo.Domain.Shared.Base;
using PtheroDemo.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;
using System.Text;
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

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdmin", policy =>
                {
                    policy.RequireRole("Admin");
                });
            });


            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<FriendlyExceptionFilter>();
            });


            #region hangfire 

            builder.Services.AddHangfire(configuration => configuration
               .UseSqlServerStorage(builder.Configuration.GetConnectionString("default")));


            #endregion 

            LoadModulesWithPrefix(builder.Host, builder.Services,builder.Configuration);

            
            //var serviceProvider = services.BuildServiceProvider();
            //builder.Services.AddScoped(typeof(IUserService), typeof(UserService));

            // ��ȡ����ʵ���� IModule �ӿڵ�����
            //var moduleTypes = Assembly.GetExecutingAssembly()
            //        .GetTypes()
            //        .Where(type => typeof(IModule).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            //        .ToList();

            // ������ʵ������Щģ�飬�������� ConfigureServices ����
            //
            //foreach (var moduleType in moduleTypes)
            //{
            //    var moduleInstance = Activator.CreateInstance(moduleType) as IModule;
            //    moduleInstance?.ConfigureServices(services);
            //}

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PtheroDemo", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.CustomOperationIds(apiDesc =>
                {
                    // ��ȡ����������
                    var controllerName = apiDesc.ActionDescriptor.RouteValues["controller"];

                    // ��ȡ��������
                    var methodName = apiDesc.HttpMethod + controllerName;

                    // ���ط���������Ϊ���� ID
                    return methodName;
                });
                //c.CustomOperationIds(apiDesc =>
                //{
                //    return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : "555";
                //});

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                };

                c.AddSecurityRequirement(securityRequirement);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<CurrentUserMiddleware>();

            // ���� Hangfire �Ǳ��
            app.UseHangfireDashboard();


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
                //containerBuilder.RegisterAssemblyTypes(service)  //�����ע��
                //    .Where(t => t.Name.EndsWith("Service"))  //��һ���ӿ��ж������ʱ�����ڹ���
                //    .AsImplementedInterfaces()
                //    .InstancePerDependency()
                //    .PropertiesAutowired(new CustomPropertySelector()); ;

                //containerBuilder.RegisterType<CustomPropertySelector>().As<IPropertySelector>();
                ////containerBuilder.RegisterApiControllers(typeof(Program).Assembly);
                ////containerBuilder.RegisterControllers(typeof(Program).Assembly);



                //// ��ȡ���з���Լ���ĳ���
                //var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"PtheroDemo.*.dll") 
                //    .Select(Assembly.LoadFrom)
                //    .ToList();



                //// ��ȡ����ģ��� IModule ʵ�ֲ����� ConfigureServices
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

                ////// ע����Щ���� 
                ////foreach (var serviceType in serviceTypes)
                ////{
                ////    var interfaces = serviceType.GetInterfaces();
                ////    foreach (var @interface in interfaces)
                ////    {
                ////        containerBuilder.RegisterType(serviceType).As(@interface).WithAttributeFiltering().InstancePerLifetimeScope();
                ////        //services.AddTransient(@interface, serviceType);
                ////    }
                ////}

                //var controllerBaseType = typeof(ControllerBase);   //������ע��
                //containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
                //    .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                //    .PropertiesAutowired(new CustomPropertySelector());

                ////var applicationModule = new ApplicationModule();
                ////applicationModule.ConfigureServices(containerBuilder, builder.Configuration);

                //// ��������ע��
                ////containerBuilder.PropertiesAutowired();
            });
        }
    }
}