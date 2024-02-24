using Microsoft.AspNetCore.Mvc;
using PtheroDemo.Application.Contract.Dtos.Department;
using PtheroDemo.Application.Contract.Dtos.Login;
using PtheroDemo.Application.Contract.IService;
using PtheroDemo.Domain.Entities;
using PtheroDemo.Domain.Shared.BaseClass;

namespace PtheroDemo.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DepartmentController : ControllerBase
    {
        public IDepartmentService DepartmentService { get; set; } 

        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<DataResult> InsertDepartment([FromBody] DepartmentDto inputDto)
        {
            var data = await DepartmentService.InsertDepartment(inputDto); 
            return data;
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task DeleteDepartmentById(long id) 
        {
            await DepartmentService.DeleteDepartment(id);
        }

        /// <summary>
        /// 获取全部部门
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DepartmentDto>> GetDepartmentList() 
        {
            var list = await DepartmentService.GetDepartmentList();
            return list;
        }

    }
}
