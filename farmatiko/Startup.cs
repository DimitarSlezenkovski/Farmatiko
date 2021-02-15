using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FarmatikoData;
using Microsoft.EntityFrameworkCore;
using FarmatikoData.FarmatikoRepoInterfaces;
using FarmatikoData.FarmatikoRepo;
using FarmatikoServices.FarmatikoServiceInterfaces;
using FarmatikoServices.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FarmatikoServices.Auth;
using FarmatikoServices.Infrastructure;
using System;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using FarmatikoServices.Services.JobDTO;

namespace Farmatiko
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                                  });
            });

            services.AddControllersWithViews();
            services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            var connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("FarmatikoConnection");
            services.AddEntityFrameworkNpgsql().AddDbContext<FarmatikoDataContext>(opt => opt.UseNpgsql(connectionString));



            services.AddScoped<IPHRepo, PHRepo>();
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IAdminRepo, AdminRepo>();
            services.AddTransient<IUpdateDataRepo, UpdateDataRepo>();

            services.AddTransient<IPHService, PHService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IService, Service>();

            services.AddTransient<IProcessJSONService, ProcessJSONService>();

            services.AddTransient<ILogger, Logger<ProcessJSONService>>();


            var jwtTokenConfig = Configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();
            services.AddSingleton(jwtTokenConfig);

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
                    ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

            /*.AddJwtBearer(cfg =>
                     {
                         cfg.RequireHttpsMetadata = false;
                         cfg.SaveToken = true;
                         cfg.IncludeErrorDetails = true;
                         cfg.TokenValidationParameters = new TokenValidationParameters()
                         {
                             ValidIssuer = Configuration.GetSection("TokenIssuer").Value,
                             ValidAudience = Configuration.GetSection("TokenIssuer").Value,
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("SecretKey").Value))
                         };

                     });
*/
            services.AddSingleton<IJwtAuthManager>(new JwtAuthManager(jwtTokenConfig));
            services.AddHostedService<JwtRefreshTokenCache>();
            services.AddScoped<IAuthService, AuthService>();
            //If we add imgs
            /*services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });*/

            services.AddScoped<UpdateDataJob>();
            services.AddSingleton<IJobFactory, SingletonUpdateDataJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddSingleton(new JobSchedule(
            jobType: typeof(UpdateDataJob),
            cronExpression: "0/30 * * * * ?"));


            // "0 0 12 */7 * ?"


            services.AddHostedService<UpdateDataHostedService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseExceptionHandler("/Error");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // if we add imgs
            /*app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
                RequestPath = new PathString("/wwwroot")
            });*/

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
