using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PtheroDemo.Application.Contract.IService;
using PtheroDemo.Domain;
using PtheroDemo.Domain.Entities;

namespace PtheroDemo.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class HomeController : ControllerBase
    {
        public IUserService UserService { get; set; } 

        private readonly ILogger<HomeController> _logger;

        
        

        //public HomeController(ILogger<HomeController> logger, IUserService userService)
        //{
        //    _logger = logger;
        //    UserService = userService;
        //}

        [HttpGet]
        public async Task<List<UserEntity>> GetUsers() 
        {
            var users =await UserService.GetUsers();
            return users;
        }

    }
}