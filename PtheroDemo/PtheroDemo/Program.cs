using Microsoft.EntityFrameworkCore;
using PtheroDemo.Application.Contract.IService;
using PtheroDemo.Application.Service;
using PtheroDemo.Domain;
using PtheroDemo.Domain.Entities;
using PtheroDemo.Domain.Shared.Base;
using PtheroDemo.EntityFrameworkCore;
using System.Reflection;

namespace PtheroDemo.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            


            // Add services to the container.

            builder.Services.AddControllers();

            //builder.Services.AddDbContext<DBContext>(db=>db.UseSqlServer(builder.Configuration.GetConnectionString("default"), b => b.MigrationsAssembly("PtheroDemo.EntityFrameworkCore")));

            //builder.Services.AddScoped(typeof(IDBContext), typeof(DBContext));

            //builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            LoadModulesWithPrefix("PtheroDemo", builder.Services,builder.Configuration);
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

        private static void LoadModulesWithPrefix(string prefix, IServiceCollection services,IConfiguration configuration)
        {
            // ��ȡ���з���Լ���ĳ���
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"{prefix}.*.dll")
                .Select(Assembly.LoadFrom)
                .ToList();

            // ��ȡ����ģ��� IModule ʵ�ֲ����� ConfigureServices
            foreach (var assembly in assemblies)
            {
                var moduleTypes = assembly.GetTypes()
                    .Where(type => typeof(IModule).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    .ToList();

                foreach (var moduleType in moduleTypes)
                {
                    var moduleInstance = Activator.CreateInstance(moduleType) as IModule;
                    moduleInstance?.ConfigureServices(services,configuration);
                }
            }
        }
    }
}