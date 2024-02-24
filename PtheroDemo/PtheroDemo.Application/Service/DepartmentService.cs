using PtheroDemo.Application.Contract.Dtos.Department;
using PtheroDemo.Application.Contract.IService;
using PtheroDemo.Domain;
using PtheroDemo.Domain.Entities;
using PtheroDemo.Domain.Shared.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Application.Service
{
    public class DepartmentService : IDepartmentService
    {
        #region ioc 

        public IRepository<DepartmentEntity,long> DepartmentRepository { get; set; } 

        #endregion 
        public async Task DeleteDepartment(long id)
        {
            await DepartmentRepository.DeleteAsync(t => t.Id == id);
        }

        public async Task<List<DepartmentEntity>> GetDepartmentList()
        {
            var query = await DepartmentRepository.GetQueryableAsync();
            return query.ToList();
        }

        public async Task<DataResult> InsertDepartment(DepartmentDto departmentDto)
        {
            try
            {
                var en = new DepartmentEntity()
                {
                    Code = departmentDto.Code,
                    Name = departmentDto.Name,
                    ParentId = departmentDto.ParentId
                };
                await DepartmentRepository.InsertAsync(en);
            }
            catch (Exception ex)
            {
                return DataResult.Failure(ex.Message);
            }
            return DataResult.Success();
        }
    }
}
