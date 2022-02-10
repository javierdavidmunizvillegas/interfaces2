using ICAJ001.api.Infraestructura.Servicios;
using ICAJ001.api.Infraestructure.Configuration;
using ICAJ001.api.Infraestructure.Services;
using ICAJ001.api.Models._001.Response;
using ICAJ001.api.Models.ResponseHomologacion;
using ICAJ001.Models._001.Request;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ001.api
{
    public class Startup
    {//VALEEEEEE
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ValidadorRequest>();
            services.AddControllers();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddScoped(typeof(IManejadorHomologacion<ResponseHomologacion>), typeof(ManejadorHomologacion<ResponseHomologacion>));
            services.AddScoped(typeof(IManejadorRequest<APICAJ001001MessageRequest>), typeof(ManejadorRequest<APICAJ001001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APICAJ001001MessageResponse>), typeof(ManejadorResponse<APICAJ001001MessageResponse>));

            

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Interfaz Documento ICAJ001",
                    Description = "Interfaz que tiene el objetivo tener la misma codificación de caja entre el sistema legado y Dynamics 365FO.",
                    Version = "v1"
                });
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
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "Swagger Demo API");
            });
        }
    }
}
