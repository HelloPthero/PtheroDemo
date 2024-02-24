using PtheroDemo.Application.Contract.Dtos.Department;
using PtheroDemo.Domain.Entities;
using PtheroDemo.Domain.Shared.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Application.Contract.IService
{
    public interface IDepartmentService
    {
        Task DeleteDepartment(long id);

        Task<List<DepartmentDto>> GetDepartmentList(); 

        Task<DataResult> InsertDepartment(DepartmentDto departmentDto);

    }
}
