using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public  Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {


            //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            // if (context.UserName == "jack" && context.Password == "123")
            if (context.Password == "123")
            {
                 context.Result = new GrantValidationResult(
                 subject: context.UserName,
                 authenticationMethod: "custom",
                 claims: GetUserClaims(context.UserName));
            }
            else
            {
                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
            return Task.CompletedTask;
        }
        //可以根据需要设置相应的Claim
        private Claim[] GetUserClaims(string username)
        {
            return new Claim[]
            {
               new Claim("UserId", 1.ToString()),
               new Claim(JwtClaimTypes.Name,username),
               new Claim(JwtClaimTypes.GivenName, "test"),
               new Claim(JwtClaimTypes.FamilyName, "test"),
               username=="jack"? new Claim(JwtClaimTypes.Role,"admin"):new Claim(JwtClaimTypes.Role,"common")
            };
        }
    }
}
