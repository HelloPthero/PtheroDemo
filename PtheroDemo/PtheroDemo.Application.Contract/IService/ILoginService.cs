using PtheroDemo.Application.Contract.Dtos.Login;
using PtheroDemo.Domain.Shared.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Application.Contract.IService
{
    public interface ILoginService
    {
        Task<DataResult> Login(LoginInputDto inputDto);
    }
}
