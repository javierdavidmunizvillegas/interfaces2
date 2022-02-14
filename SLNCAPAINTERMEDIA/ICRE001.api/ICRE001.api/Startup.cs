using ICRE001.api.Infraestructura.Servicios;
using ICRE001.api.Infraestructure.Configuration;
using ICRE001.api.Infraestructure.Services;
using ICRE001.api.Models.ResponseHomologacion;
using ICRE001.Models;
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

namespace ICRE001.api
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

            services.AddScoped(typeof(IManejadorRequest<APICRE001001MessageRequest>), typeof(ManejadorRequest<APICRE001001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APICRE001001MessageResponse>), typeof(ManejadorResponse<APICRE001001MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APICRE001002MessageRequest>), typeof(ManejadorRequest<APICRE001002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APICRE001002MessageResponse>), typeof(ManejadorResponse<APICRE001002MessageResponse>));

            services.AddScoped<ValidadorRequest001>();
            services.AddScoped<ValidadorRequest002>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Interfaz Documento ICRE001",
                Description = "Captación de Solicitudes de Crédito por Distribuidores Independientes Multinova",
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
