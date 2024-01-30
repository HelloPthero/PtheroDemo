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
        public void ConfigureServices(IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<DBContext>(db => db.UseSqlServer(configuration.GetConnectionString("default")/*, b => b.MigrationsAssembly("PtheroDemo.EntityFrameworkCore")*/));

            services.AddScoped(typeof(IDBContext), typeof(DBContext));
        }
    }
}
