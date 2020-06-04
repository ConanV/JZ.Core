using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JZ.Core.Utility;
using JZ.Core.Models;

namespace JZ.Core.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly SchoolsDBContext _dbContext;

        public TeacherController(SchoolsDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetTeachersList")]
        public ResponseMessage<List<TTeacher>> GetTeachersList()
        {
            dynamic result = null;
            var list = _dbContext.TTeacher.ToList();

            result = CreateResult.For(list, list.Count);

            return result;
        }
    }
}