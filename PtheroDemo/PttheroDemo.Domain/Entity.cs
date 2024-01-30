using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain
{
    public class Entity<K> : IEntity<K> where K : struct
    {
        public K Id { get; set; } 
    }
}
