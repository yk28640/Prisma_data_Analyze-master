using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApi.Authenticaion
{
    public class JwtTokenUtil
    {
        private readonly IConfiguration _configuration;

        public JwtTokenUtil(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetToken(string userName)
        {
            // push the user’s name into a claim, so we can identify the user later on.
            var claims = new[]
            {
                   new Claim(ClaimTypes.Name, userName),
                   //new Claim(ClaimTypes.Role, "Admin")//在这可以分配用户角色，比如管理员 、 vip会员 、 普通用户等
                };
            //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"])); // 获取密钥
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //凭证 ，根据密钥生成
            //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
            /**
             * Claims (Payload)
                Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:

                iss: The issuer of the token，token 是给谁的  发送者
                aud: 接收的
                sub: The subject of the token，token 主题
                exp: Expiration Time。 token 过期时间，Unix 时间戳格式
                iat: Issued At。 token 创建时间， Unix 时间戳格式
                jti: JWT ID。针对当前 token 的唯一标识
                除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
             * */
            var token = new JwtSecurityToken(
                
                issuer: "igbom_web",
                audience: "igbom_web",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );;

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
