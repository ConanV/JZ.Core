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


            //ԭ��������ȡ���ݿ�
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SchoolsDBContext>(options =>options.UseSqlServer(connectionString));
            //ע��Dapper
            services.AddDapper("SqlDb", m =>
            {
                m.ConnectionString = connectionString;
                m.DbType = DbStoreType.SqlServer;
            });
            //log4net
            repository = LogManager.CreateRepository("CoreLogRepository");
            XmlConfigurator.Configure(repository, new FileInfo("config/log4net.config"));
            Log4NetRepository.loggerRepository = repository;

            #region ע�� Swagger
            services.AddSwaggerGen(sg =>
            {
                sg.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v0.1.0",
                    Title = "JZ.Core.WebAPI",
                    Description = "JZ.Core.WebAPI���˵���ĵ�",
                }) ;
                //��Ӷ�ȡע�ͷ���
                sg.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
                sg.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JZ.Core.Models.xml"));
            });
            #endregion

            services.AddControllers(opt =>
            {//ȫ���쳣����
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

            #region ע��Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                c.RoutePrefix = "";//·�����ã�����Ϊ�գ���ʾֱ�ӷ��ʸ��ļ���
                                   //·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,ע��localhost:8001/swagger�Ƿ��ʲ����ģ�
                                   //���ʱ��ȥlaunchSettings.json�а�"launchUrl": "swagger/index.html"ȥ���� Ȼ��ֱ�ӷ���localhost:8001/index.html����
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
