using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PtheroDemo.Application.Contract.Dtos.User;
using PtheroDemo.Application.Contract.IService;
using PtheroDemo.Domain;
using PtheroDemo.Domain.Entities;

namespace PtheroDemo.Host.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        public IUserService UserService { get; set; } 

        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// ��ȡ�����û�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<UserDto>> GetUsers() 
        {
            var users =await UserService.GetUsers();
            return users;
        }

        /// <summary>
        /// ��ȡ��ǰ��¼�û�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public UserDto GetCurrentUser()
        {
            return UserService.GetCurrent();
        }

    }
}