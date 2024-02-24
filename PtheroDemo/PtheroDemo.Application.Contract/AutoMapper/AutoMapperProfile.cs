using AutoMapper;
using PtheroDemo.Application.Contract.Dtos.Department;
using PtheroDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Application.Contract.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<DepartmentEntity, DepartmentDto>()
                .ReverseMap();
        }
    }
}
