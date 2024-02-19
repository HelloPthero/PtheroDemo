using Microsoft.AspNetCore.Mvc;
using PtheroDemo.Application.Contract.Dtos.Login;
using PtheroDemo.Application.Contract.IService;
using PtheroDemo.Application.Service;
using PtheroDemo.Domain.Entities;
using PtheroDemo.Domain.Shared.BaseClass;

namespace PtheroDemo.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController:ControllerBase
    {
        public ILoginService LoginService { get; set; }

        [HttpPost]
        public async Task<DataResult> Login([FromBody] LoginInputDto inputDto)
        {
            var data = await LoginService.Login(inputDto);
            return data; 
        }
    }
}
