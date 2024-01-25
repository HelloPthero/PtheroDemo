using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PtheroDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.EntityFrameworkCore.EntityConfiguration
{
    public class MenuEntityConfiguration : IEntityConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuEntity>()
                .ToTable("T_Menu")
               .HasKey(e => e.Id);
        }
    }
}
