using PtheroDemo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain.Entities
{
    public class UserEntity : Entity<long>
    {

        public string Name { get; set; }

        public string Password { get; set; }
    }
}
