using DatingApp.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using DatingApp.API.Helpers;
using AutoMapper;

namespace DatingApp.API
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
            services.AddDbContext<DataContext>(x => 
            x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddCors();

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IDatingRepository, DatingRepository>(); // Common repository
            services.AddScoped<LogUserActivity>();

            services.AddAutoMapper(typeof(DatingRepository).Assembly); // Just give any one file's assembly here.

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
             AddJwtBearer(options => // Action
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                     .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                     ValidateIssuer = false,
                     ValidateAudience = false
                 };
             });

            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
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
                app.UseExceptionHandler(builder => //Example of an Action
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message); // Our Extension method
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseDefaultFiles(); // looks for and loads the index.html
            app.UseStaticFiles(); // looks for angular files in wwwroot folder

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback"); 
            });
        }
    }
}
