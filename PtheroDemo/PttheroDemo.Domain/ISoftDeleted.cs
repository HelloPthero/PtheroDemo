﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain
{

    internal interface ISoftDeleted
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedTime { get; set; }

        public long? DeleteUserId { get; set; } 
    }
}
