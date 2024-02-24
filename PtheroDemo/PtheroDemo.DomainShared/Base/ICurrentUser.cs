using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain.Shared.Base
{
    public interface ICurrentUser
    {
        int UserId { get; }
        string UserName { get; }
    }
}
