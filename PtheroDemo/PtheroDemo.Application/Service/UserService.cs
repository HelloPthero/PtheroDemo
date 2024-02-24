using PtheroDemo.Application.Contract.IService;
using PtheroDemo.Domain;
using PtheroDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.AttributeMetadata;
using PtheroDemo.Domain.Shared.Base;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PtheroDemo.Application.Contract.Dtos.User;
using AutoMapper;
using PtheroDemo.Application.Base;

namespace PtheroDemo.Application.Service
{
    public class UserService : ServiceBase, IUserService
    {
        //[PropertyInject]
        public IRepository<UserEntity, long> UserRepository { get; set; }

        public IMapper AutoMapper { get; set; }

        public async Task<List<UserDto>> GetUsers() 
        {
            var users = (await UserRepository.GetQueryableAsync()).ToList();
            return AutoMapper.Map<List<UserDto>>(users);
        }

        public UserDto GetCurrent()
        {
            var currentUser = base.GetCurrentUser();
            var dto = new UserDto()
            {
                Id = currentUser.UserId,
                Name = currentUser.UserName
            };
            return dto;
        }
    }
}
