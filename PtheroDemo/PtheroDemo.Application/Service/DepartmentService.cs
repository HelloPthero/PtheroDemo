using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using PtheroDemo.Application.Base;
using PtheroDemo.Application.Contract.Dtos.Department;
using PtheroDemo.Application.Contract.IService;
using PtheroDemo.Domain;
using PtheroDemo.Domain.Entities;
using PtheroDemo.Domain.Shared.Base;
using PtheroDemo.Domain.Shared.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PtheroDemo.Application.Service
{
    public class DepartmentService : ServiceBase, IDepartmentService
    {
        #region ioc 

        public IRepository<DepartmentEntity,long> DepartmentRepository { get; set; }

        public IDistributedCache RedisCache { get; set; } 

        public IMapper AutoMapper { get; set; }  

        #endregion 
        public async Task DeleteDepartment(long id)
        {
            await DepartmentRepository.DeleteAsync(t => t.Id == id);
        }

        public async Task<List<DepartmentDto>> GetDepartmentList()
        {
            var cacheKey = "departments";
            var departments = await RedisCache.GetStringAsync(cacheKey);
            if (departments != null)
            {
                return JsonSerializer.Deserialize<List<DepartmentDto>>(departments);
            }
            else
            {
                var query = await DepartmentRepository.GetQueryableAsync();
                var list = AutoMapper.Map<List<DepartmentDto>>(query);
                await RedisCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(list), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
                return list;
            }
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
