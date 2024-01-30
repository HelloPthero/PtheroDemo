using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain
{
    public interface IDBContext
    {
        DbSet<T> Set<T>() where T : class;

        Task<int>  SaveChangesAsync();

        EntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
