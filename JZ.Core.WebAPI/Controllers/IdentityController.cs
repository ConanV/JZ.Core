// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using JZ.Core.Utility;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JZ.Core.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public ResponseMessage<IEnumerable<Dictionary<string, string>>> Get()
        {
            var list = from c in User.Claims select new Dictionary<string, string> { { c.Type, c.Value } };

            return CreateResult.For(list);
        }
    }

    
}
