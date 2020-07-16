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
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;

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
           services.AddAuthentication("Bearer")
           .AddJwtBearer("Bearer", options =>
           {
               options.Authority = "https://localhost:44300";

               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateAudience = false
               };
           });
           services.AddAuthorization(options =>
           {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api1");
                });
           });


            //原生方法获取数据库
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SchoolsDBContext>(options =>options.UseSqlServer(connectionString));
            //注入Dapper
            services.AddDapper("SqlDb", m =>
            {
                m.ConnectionString = connectionString;
                m.DbType = DbStoreType.SqlServer;
            });
            //log4net
            repository = LogManager.CreateRepository("CoreLogRepository");
            XmlConfigurator.Configure(repository, new FileInfo("config/log4net.config"));
            Log4NetRepository.loggerRepository = repository;

            #region 注册 Swagger
            services.AddSwaggerGen(sg =>
            {
                sg.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v0.1.0",
                    Title = "JZ.Core.WebAPI",
                    Description = "JZ.Core.WebAPI框架说明文档",
                }) ;
                //添加读取注释服务
                sg.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
                sg.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JZ.Core.Models.xml"));
            });
            #endregion

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

            #region 注册Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                c.RoutePrefix = "";//路径配置，设置为空，表示直接访问该文件，
                                   //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，
                                   //这个时候去launchSettings.json中把"launchUrl": "swagger/index.html"去掉， 然后直接访问localhost:8001/index.html即可
            });
            #endregion

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization("ApiScope");
            });

        }
    }
}
