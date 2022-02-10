using ICAJ005.api.Infraestructura.Servicios;
using ICAJ005.api.Infraestructure.Configuration;
using ICAJ005.api.Infraestructure.Services;
using ICAJ005.api.Models._001.Request;
using ICAJ005.api.Models._001.Response;
using ICAJ005.api.Models.ResponseHomologacion;
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

namespace ICAJ005.api
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
            services.AddScoped<ValidadorRequest>();
            services.AddControllers();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddScoped(typeof(IManejadorHomologacion<ResponseHomologacion>), typeof(ManejadorHomologacion<ResponseHomologacion>));
            services.AddScoped(typeof(IManejadorRequest<APICAJ005001MessageRequest>), typeof(ManejadorRequest<APICAJ005001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APICAJ005001MessageResponse>), typeof(ManejadorResponse<APICAJ005001MessageResponse>));

            

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Interfaz Documento ICAJ005",
                Description = "Interfaz para la confirmaci�n de dep�sito de valores se realiza una vez que se entrega el dep�sito de valores al banco (Blindado o ventanilla), y se confirma en el sistema luego haberse recibido el dep�sito",
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
