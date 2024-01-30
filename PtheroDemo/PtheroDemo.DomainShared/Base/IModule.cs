using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain.Shared.Base
{
    public interface IModule
    {
        void ConfigureServices(IServiceCollection services,IConfiguration configuration);
    }
}
