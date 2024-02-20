using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PtheroDemo.Application.Contract.Dtos.Login;
using PtheroDemo.Application.Contract.IService;
using PtheroDemo.Domain;
using PtheroDemo.Domain.Entities;
using PtheroDemo.Domain.Shared.BaseClass;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Application.Service
{
    public class LoginService : ILoginService
    {
        public IRepository<UserEntity,long> UserRepository { get; set; } 

        public IConfiguration configuration { get; set; }



        public async Task<DataResult> Login(LoginInputDto inputDto)
        {
            var user = (await UserRepository.GetQueryableAsync(t => t.Name == inputDto.Name && t.Password == inputDto.Password)).FirstOrDefault();
            if (user == null)
            {
                return DataResult.Failure("校验失败");
            }
            var token = GenerateJwtToken(user);
            return DataResult.Success(data: token);
        }

        private string GenerateJwtToken(UserEntity user) 
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                   new(ClaimTypes.Name, user.Name),
                    // 还可以添加其他的声明，如用户ID、角色等
                }),
                Expires = DateTime.UtcNow.AddHours(1), // 设置令牌过期时间
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
