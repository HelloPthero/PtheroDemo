using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Application.Contract.Dtos.Department
{
    public class DepartmentDto
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父级节点
        /// </summary>
        public long ParentId { get; set; }
    }
}
