using Microsoft.EntityFrameworkCore;
using PtheroDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.EntityFrameworkCore.EntityConfiguration
{
    public class DepartmentEntityConfiguration : IEntityConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepartmentEntity>()
               .ToTable("T_Department")
               .HasKey(e => e.Id);
        }

    }
}
