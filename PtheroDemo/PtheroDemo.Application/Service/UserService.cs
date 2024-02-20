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

namespace PtheroDemo.Application.Service
{
    public class UserService : IUserService
    {
        //[PropertyInject]
        public IRepository<UserEntity, long> UserRepository { get; set; }

        //public UserService(IRepository<UserEntity, long> userRepository)
        //{
        //    UserRepository = userRepository;
        //}

        public async Task<List<UserEntity>> GetUsers() 
        {
            var users = (await UserRepository.GetQueryableAsync()).ToList();
            return users;
        }
    }
}
