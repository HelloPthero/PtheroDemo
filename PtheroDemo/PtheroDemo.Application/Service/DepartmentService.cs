﻿using AutoMapper;
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
using System.Threading.Tasks;

namespace PtheroDemo.Application.Service
{
    public class DepartmentService : IDepartmentService
    {
        #region ioc 

        public IRepository<DepartmentEntity,long> DepartmentRepository { get; set; }

        public IMapper AutoMapper { get; set; }  

        #endregion 
        public async Task DeleteDepartment(long id)
        {
            await DepartmentRepository.DeleteAsync(t => t.Id == id);
        }

        public async Task<List<DepartmentDto>> GetDepartmentList()
        {
            var query = await DepartmentRepository.GetQueryableAsync();
            var list = AutoMapper.Map<List<DepartmentDto>>(query);
            return list;
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
