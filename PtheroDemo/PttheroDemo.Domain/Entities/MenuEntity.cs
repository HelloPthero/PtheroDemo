using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain.Entities
{
    public class MenuEntity:Entity<long>
    {
        public string Name { get; set; }

        public long ParentId { get; set; } 
    }
}
