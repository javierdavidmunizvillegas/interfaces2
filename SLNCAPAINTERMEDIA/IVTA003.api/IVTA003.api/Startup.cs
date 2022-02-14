using ICXP003.api.Infraestructura.Servicios;
using IVTA003.api.Infraestructure.Configuration;
using IVTA003.api.Infraestructure.Services;
using IVTA003.api.Models._001.Request;
using IVTA003.api.Models._001.Response;
using IVTA003.api.Models._002.Request;
using IVTA003.api.Models._002.Response;
using IVTA003.api.Models._003.Request;
using IVTA003.api.Models._003.Response;
using IVTA003.api.Models._004.Request;
using IVTA003.api.Models._004.Response;
using IVTA003.api.Models._005.Request;
using IVTA003.api.Models._005.Response;
using IVTA003.api.Models.ResponseHomologacion;
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

namespace IVTA003.api
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

            services.AddScoped(typeof(IManejadorRequest<APIVTA003001MessageRequest>), typeof(ManejadorRequest<APIVTA003001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIVTA003001MessageResponse>), typeof(ManejadorResponse<APIVTA003001MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APIVTA003002MessageRequest>), typeof(ManejadorRequest<APIVTA003002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIVTA003002MessageResponse>), typeof(ManejadorResponse<APIVTA003002MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APIVTA003003MessageRequest>), typeof(ManejadorRequest<APIVTA003003MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIVTA003003MessageResponse>), typeof(ManejadorResponse<APIVTA003003MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APIVTA003004MessageRequest>), typeof(ManejadorRequest<APIVTA003004MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIVTA003004MessageResponse>), typeof(ManejadorResponse<APIVTA003004MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APIVTA003005MessageRequest>), typeof(ManejadorRequest<APIVTA003005MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIVTA003005MessageResponse>), typeof(ManejadorResponse<APIVTA003005MessageResponse>));

            services.AddScoped<ValidadorRequest001>();
            services.AddScoped<ValidadorRequest002>();
            services.AddScoped<ValidadorRequest003>();
            services.AddScoped<ValidadorRequest004>();
            services.AddScoped<ValidadorRequest005>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Interfaz Documento IVTA003",
                Description = "Interfaz para consulta de clientes, consulta del maestro de productos, consulta de inventario propio y consignado, consulta de almacenes / zonas",
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
