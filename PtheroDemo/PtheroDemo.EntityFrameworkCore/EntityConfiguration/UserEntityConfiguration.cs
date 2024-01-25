using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PtheroDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.EntityFrameworkCore.EntityConfiguration
{
    public class UserEntityConfiguration : IEntityConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .ToTable("T_User")
                .HasKey(e => e.Id);

            modelBuilder.Entity<UserEntity>()
                .Property(t => t.Name).HasMaxLength(100);
        }
    }
}
