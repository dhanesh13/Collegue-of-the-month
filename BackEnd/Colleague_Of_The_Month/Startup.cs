using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Colleague_Of_The_Month.Models;
using Colleague_Of_The_Month.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Colleague_Of_The_Month.Email;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Colleague_Of_The_Month.Interfaces;

namespace Colleague_Of_The_Month
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var emailconfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();

            services.AddSession();
            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();
            services.AddCors();
            services.AddControllers();
            services.AddDbContext<COTMDBContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("Connection")));
            services.AddSwaggerGen(
               c =>
               {
                   c.SwaggerDoc("v1", new OpenApiInfo { Title = "Colleague Of The Month project", Version = "v1" });
                   c.CustomSchemaIds(x => x.FullName);
               });
            
            
            services.AddScoped<INominee, COTMService>();
            services.AddScoped<IDetails, COTMService>();
            services.AddScoped<IDateAccess, COTMService>();
            services.AddScoped<IBusinessUnit, COTMService>();
            services.AddScoped<ICostCentre, COTMService>();
            services.AddScoped<IDepartment, COTMService>();
            services.AddScoped<IDivision, COTMService>();
            services.AddScoped<IEmployee, COTMService>();
            services.AddScoped<IEvent, COTMService>();
            services.AddScoped<ISubdivision, COTMService>();
            services.AddScoped<IVoucher, COTMService>();
            services.AddScoped<IMail, COTMService>();
            services.AddScoped<IVotes, VoteService>();
            services.AddScoped<ILogin, LoginService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddSingleton(emailconfig);

            services.AddDirectoryBrowser();
            
            services.AddScoped<IUpload, UploadService>();
            services.AddScoped<IInspireTeam, InspireTeamService>();


            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(app => app
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            );
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TEST");
            });

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
                RequestPath = new PathString("/wwwroot")
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseStaticFiles();
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images")),
                RequestPath = new PathString("/COTMImages")
            });
        }
    }
}
