using IPRO002.api.Infraestructura.Servicios;
using IPRO002.api.Infraestructure.Configuration;
using IPRO002.api.Infraestructure.Services;
using IPRO002.api.Models._001.Request;
using IPRO002.api.Models._001.Response;
using IPRO002.api.Models._002.Request;
using IPRO002.api.Models._002.Response;
using IPRO002.api.Models._003.Request;
using IPRO002.api.Models._003.Response;
using IPRO002.api.Models._004.Request;
using IPRO002.api.Models._004.Response;
using IPRO002.api.Models.ResponseHomologacion;
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

namespace IPRO002.api
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

            services.AddScoped(typeof(IManejadorRequest<APIPRO002001MessageRequest>), typeof(ManejadorRequest<APIPRO002001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO002001MessageResponse>), typeof(ManejadorResponse<APIPRO002001MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APIPRO002002MessageRequest>), typeof(ManejadorRequest<APIPRO002002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO002002MessageResponse>), typeof(ManejadorResponse<APIPRO002002MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APIPRO002003MessageRequest>), typeof(ManejadorRequest<APIPRO002003MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO002003MessageResponse>), typeof(ManejadorResponse<APIPRO002003MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APIPRO002004MessageRequest>), typeof(ManejadorRequest<APIPRO002004MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO002004MessageResponse>), typeof(ManejadorResponse<APIPRO002004MessageResponse>));

            services.AddScoped<ValidadorRequest001>();
            services.AddScoped<ValidadorRequest002>();
            services.AddScoped<ValidadorRequest003>();
            services.AddScoped<ValidadorRequest004>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Interfaz Documento IPRO002",
                Description = "Interfaz entre R y Dynamics 365 para generar información inclusión de producto. ",
                Version = "v1"
            });
            });
        }

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
