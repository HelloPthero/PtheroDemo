using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain.Shared.Base
{
    /// <summary>
    /// 友好异常
    /// </summary>
    public class FriendlyException:Exception
    {
        public FriendlyException(string message):base(message) 
        { 
            
        }
    }
}
