using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class ProfileService: IProfileService
    {
        public  Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //depending on the scope accessing the user data.
                var claims = context.Subject.Claims.ToList();

                //set issued claims to return
                context.IssuedClaims = claims.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
                //log your error
            }

            return Task.CompletedTask;
        }
        /// <summary>
        /// 验证用户是否有效 例如：token创建或者验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        public Task IsActiveAsync(IsActiveContext context)
        { 
            context.IsActive = true;

            return Task.CompletedTask;
        }
    }
}
