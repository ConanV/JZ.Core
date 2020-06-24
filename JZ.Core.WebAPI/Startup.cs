using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JZ.Core.Models;
using Microsoft.EntityFrameworkCore;
using JZ.DapperManager;
using log4net.Repository;
using log4net;
using log4net.Config;
using JZ.Core.Utility.Log4Net;
using System.IO;

namespace JZ.Core.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static ILoggerRepository repository { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //原生方法获取数据库
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SchoolsDBContext>(options =>options.UseSqlServer(connectionString));
            //注入Dapper
            services.AddDapper("SqlDb", m =>
            {
                m.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                m.DbType = DbStoreType.SqlServer;
            });
            //log4net
            repository = LogManager.CreateRepository("CoreLogRepository");
            XmlConfigurator.Configure(repository, new FileInfo("config/log4net.config"));
            Log4NetRepository.loggerRepository = repository;

            services.AddControllers(opt =>
            {//全局异常捕获
                opt.Filters.Add<ApiExceptionFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
