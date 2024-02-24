using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain.Entities
{
    public class DepartmentEntity : Entity<long>, ISoftDeleted
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public long ParentId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedTime { get; set; }
        public long? DeleteUserId { get; set; }
    }
}
