using ICRE002.api.Infraestructura.Servicios;
using ICRE002.api.Infraestructure.Configuration;
using ICRE002.api.Infraestructure.Services;
using ICRE002.api.Models;
using ICRE002.api.Models._002.Request;
using ICRE002.api.Models._002.Response;
using ICRE002.api.Models._003.Request;
using ICRE002.api.Models._003.Response;
using ICRE002.api.Models.Response;
using ICRE002.api.Models.ResponseHomologacion;
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

namespace ICRE002.api
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


            services.AddScoped(typeof(IManejadorRequest<APICRE002001MessageRequest>), typeof(ManejadorRequest<APICRE002001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APICRE002001MessageResponse>), typeof(ManejadorResponse<APICRE002001MessageResponse>));
            services.AddScoped(typeof(IManejadorRequest<APICRE002002MessageRequest>), typeof(ManejadorRequest<APICRE002002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APICRE002002MessageResponse>), typeof(ManejadorResponse<APICRE002002MessageResponse>));
            services.AddScoped(typeof(IManejadorRequest<APICRE002003MessageRequest>), typeof(ManejadorRequest<APICRE002003MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APICRE002003MessageResponse>), typeof(ManejadorResponse<APICRE002003MessageResponse>));

            services.AddScoped<ValidadorRequest001>();
            services.AddScoped<ValidadorRequest002>();
            services.AddScoped<ValidadorRequest003>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Interfaz documento ICRE002",
                    Description = "Interfaz con SIAC para la actualización de datos de clientes, individual y/o masiva, a través del consumo de un servicio web donde SIAC Crédito dará el input con el código de cliente del que se requiere actualizar su información",
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
