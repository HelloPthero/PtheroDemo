using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PtheroDemo.Domain;
using PtheroDemo.Domain.Entities;
using PtheroDemo.EntityFrameworkCore.EntityConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.EntityFrameworkCore
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 指定包含实体的程序集的名称
            var entityAssemblyName = "PtheroDemo.Domain";

            base.OnModelCreating(modelBuilder);

            // 指定包含实体和配置的程序集的名称
            //var assemblyName = "YourEntityAndConfigurationAssemblyName";

            // 加载包含实体和配置的程序集
            var entityAssembly = Assembly.Load(entityAssemblyName); 

            var entityTypes = entityAssembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(IEntity<>).IsAssignableFrom(type));

            foreach (var entityType in entityTypes)
            {
                var entityMethod = typeof(ModelBuilder).GetMethod("Entity", new Type[] { });
                var genericEntityMethod = entityMethod.MakeGenericMethod(entityType);
                var entityBuilder = genericEntityMethod.Invoke(modelBuilder, null);

                // 获取主键类型
                var primaryKeyType = entityType.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntity<>))
                    .GetGenericArguments()[0];

                // 配置主键
                var keyMethod = typeof(EntityTypeBuilder<>).MakeGenericType(entityType).GetMethod("HasKey");
                var genericKeyMethod = keyMethod.MakeGenericMethod(primaryKeyType);
                genericKeyMethod.Invoke(entityBuilder, new object[] { entityType.GetProperty("Id") });

                // 在这里可以根据需要配置实体的其他属性、关系等信息
            }

            var configurationTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(IEntityConfiguration).IsAssignableFrom(type));

            foreach (var configurationType in configurationTypes)
            {
                var configuration = (IEntityConfiguration)Activator.CreateInstance(configurationType);
                configuration.Configure(modelBuilder);
            }
        }
    }
}
