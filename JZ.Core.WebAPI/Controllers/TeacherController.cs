using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JZ.Core.Utility;
using JZ.Core.Models;
using JZ.DapperManager;

namespace JZ.Core.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly SchoolsDBContext _dbContext;

        private readonly DapperClient _SqlDB;

        public TeacherController(SchoolsDBContext dbContext, IDapperFactory dapperFactory)
        {
            _dbContext = dbContext;

            _SqlDB = dapperFactory.CreateClient("SqlDb");
        }

        [HttpGet("GetTeachersList")]
        public ResponseMessage<List<TTeacher>> GetTeachersList()
        {
            dynamic result;
            var list = _dbContext.TTeacher.ToList();

            result = CreateResult.For(list, list.Count);

            return result;
        }

        [HttpGet("GetTeachersList_D")]
        public ResponseMessage<List<T_Teacher>> GetTeachersList_D()
        {
            dynamic result;
            var list = _SqlDB.Query<T_Teacher>("SELECT * FROM T_Teacher");

            result = CreateResult.For(list, list.Count);

            return result;
        }

        [HttpGet("GetTeachersListById_D")]
        public ResponseMessage<T_Teacher> GetTeachersListById_D(int id)
        {
            dynamic result;
            var m = _SqlDB.QueryFirst<T_Teacher>("SELECT * FROM T_Teacher WHERE F_ID =@id", new { id });

            result = CreateResult.For(m);

            return result;
        }

        [HttpGet("GetTeachersListByName_D")]
        public ResponseMessage<List<T_Teacher>> GetTeachersListByName_D(string name)
        {
            dynamic result;

            var list = _SqlDB.Query<T_Teacher>("SELECT * FROM T_Teacher WHERE F_TeacherName LIKE @name",
                new { name = "%" + name + "%" });

            result = CreateResult.For(list, list.Count);

            return result;
        }

        [HttpGet("InsertTeacher")]
        public ResponseMessage<object> InsertTeacher()
        {
            dynamic result;
            var teacher = new T_Teacher()
            {
                F_TeacherName = $"Test_{ (DateTime.Now.Ticks - 621356256000000000) / 10000}"
            };
            var n = _SqlDB.Excute($"INSERT INTO T_Teacher (F_TeacherName) VALUES(@F_TeacherName)",
                          teacher);

            if (n > 0)
                result = CreateResult.For();
            else
                result = CreateResult.For("00001", "新增出现错误");

            return result;
        }

        [HttpGet("GetClassAndTeachersListByName_D")]
        public ResponseMessage<List<T_Class>> GetClassAndTeachersListByName_D(string name)
        {
            dynamic result;
            var e = new Dictionary<int, T_Class>();

            var list = _SqlDB.Query<T_Class, T_Teacher, T_Class>(@"SELECT * FROM T_Class AS c
                                                    JOIN T_Teacher AS t ON c.F_TID=t.F_ID WHERE c.F_ClassName LIKE @name",
                        map: (c, t) =>
                        {
                            T_Class entity;
                            if (!e.TryGetValue(c.F_ID.Value, out entity))
                            {
                                entity = c;
                                entity.Teacher = t;

                            }
                            return entity;
                        },
                        param: new { name = "%" + name + "%" },
                        splitOn: "F_TID");

            result = CreateResult.For(list, list.Count);

            return result;
        }
    }
}