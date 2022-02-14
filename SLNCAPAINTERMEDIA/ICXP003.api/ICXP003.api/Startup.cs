using ICXP003.api.Infraestructura.Servicios;
using ICXP003.api.Infraestructure.Configuration;
using ICXP003.api.Infraestructure.Services;
using ICXP003.api.Models._001.Request;
using ICXP003.api.Models._001.Response;
using ICXP003.api.Models._002.Request;
using ICXP003.api.Models._002.Response;
using ICXP003.api.Models.ResponseHomologacion;
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

namespace ICXP003.api
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

            services.AddScoped(typeof(IManejadorRequest<APICXP003001MessageRequest>), typeof(ManejadorRequest<APICXP003001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APICXP003001MessageResponse>), typeof(ManejadorResponse<APICXP003001MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APICXP003002MessageRequest>), typeof(ManejadorRequest<APICXP003002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APICXP003002MessageResponse>), typeof(ManejadorResponse<APICXP003002MessageResponse>));

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
                Title = "Interfaz Documento ICXP003",
                Description = "Pagos a proveedores independientes Multinova",
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
