using ICAJ024.api.Infraestructura.Servicios;
using ICAJ024.api.Infraestructure.Configuration;
using ICAJ024.api.Infraestructure.Services;
using ICAJ024.api.Models._001.Request;
using ICAJ024.api.Models._001.Response;
using ICAJ024.api.Models.ResponseHomologacion;
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

namespace ICAJ024.api
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
            services.AddControllers();
            services.AddScoped(typeof(IManejadorHomologacion<ResponseHomologacion>), typeof(ManejadorHomologacion<ResponseHomologacion>));
            services.AddScoped(typeof(IManejadorRequest<APICAJ024001MessageRequest>), typeof(ManejadorRequest<APICAJ024001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APILOG002001MessageResponse>), typeof(ManejadorResponse<APILOG002001MessageResponse>));

            services.AddScoped<ValidadorRequest>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Interfaz Documento ICAJ024",
                Description = "Generaci�n de notas de d�bitos para la anulaci�n de notas de cr�ditos de billete comprador",
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
                        pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "Swagger Demo API");
            });
        }
    }
}
