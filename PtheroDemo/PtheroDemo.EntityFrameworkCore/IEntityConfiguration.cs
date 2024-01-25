using Microsoft.EntityFrameworkCore;
using PtheroDemo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.EntityFrameworkCore
{
    public interface IEntityConfiguration
    {
        public void Configure(ModelBuilder modelBuilder);
    }
}
