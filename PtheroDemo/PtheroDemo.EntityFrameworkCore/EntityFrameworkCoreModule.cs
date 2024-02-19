using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PtheroDemo.Domain;
using PtheroDemo.Domain.Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.EntityFrameworkCore
{
    public class EntityFrameworkCoreModule : IModule
    {
        public void ConfigureServices(ContainerBuilder containerBuilder, IServiceCollection services,IConfiguration configuration)
        {
            containerBuilder.RegisterType<DBContext>()
               .AsSelf()  // 注册 DbContext 类型本身
               .As<IDBContext>()  // 注册接口 IDBContext
               .WithParameter("options", GetDbContextOptions(configuration))  // 通过 WithParameter 传递 DbContextOptions
               .InstancePerLifetimeScope()
               ; // 生命周期为每次请求一个实例

            //services.AddDbContext<DBContext>(db => db.UseSqlServer(configuration.GetConnectionString("default")/*, b => b.MigrationsAssembly("PtheroDemo.EntityFrameworkCore")*/));

            //services.AddScoped(typeof(IDBContext), typeof(DBContext));
        }

        private DbContextOptions<DBContext> GetDbContextOptions(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("default");

            var optionsBuilder = new DbContextOptionsBuilder<DBContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return optionsBuilder.Options;
        }
    }

    

    
}
