using ICAJ012.api.Infraestructure.Configuration;
using ICAJ012.api.Infraestructure.Services;
using ICAJ012.api.Models._001.Request;
using ICAJ012.api.Models._001.Response;
using ICAJ012.api.Models._002.Request;
using ICAJ012.api.Models._002.Response;
using ICAJ012.api.Models.Homologacion;
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

namespace ICAJ012
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
            services.AddScoped<ValidadorRequest001>();
            services.AddScoped<ValidadorRequest002>();
            services.AddControllers();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddScoped(typeof(IManejadorHomologacion<ResponseHomologacion>), typeof(ManejadorHomologacion<ResponseHomologacion>));
            services.AddScoped(typeof(IManejadorRequest<APICAJ012001MessageRequest>), typeof(ManejadorRequest<APICAJ012001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APICAJ012001MessageResponse>), typeof(ManejadorResponse<APICAJ012001MessageResponse>));
            services.AddScoped(typeof(IManejadorRequest<APICAJ012002MessageRequest>), typeof(ManejadorRequest<APICAJ012002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APICAJ012002MessageResponse>), typeof(ManejadorResponse<APICAJ012002MessageResponse>));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Interfaz Documento ICAJ012",
                Description = "Interfaz con SIAC Caja de consulta del parametrizador de motivos de formas de pago de egreso y su especificación.",
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
                     // pattern: "{controller=Home}/{action=Index}/{id?}");

                     pattern: "{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "DEmo");

            });
        }
    }
}
