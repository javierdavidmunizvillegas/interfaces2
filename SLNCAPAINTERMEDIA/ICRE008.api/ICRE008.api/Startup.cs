using ICRE008.api.Infraestructura.Servicios;
using ICRE008.api.Infraestructure.Configuration;
using ICRE008.api.Infraestructure.Services;
using ICRE008.api.Models._001.Request;
using ICRE008.api.Models._001.Response;
using ICRE008.api.Models.ResponseHomologacion;
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

namespace ICRE008.api
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
            services.AddControllers();
            services.AddScoped(typeof(IManejadorHomologacion<ResponseHomologacion>), typeof(ManejadorHomologacion<ResponseHomologacion>));
            services.AddScoped(typeof(IManejadorRequest<APICRE009001MessageRequest>), typeof(ManejadorRequest<APICRE009001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APICRE009001MessageResponse>), typeof(ManejadorResponse<APICRE009001MessageResponse>));

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
                Title = "Interfaz Documento ICRE008",
                Description = "Interfaz con MSD365FO que le permita consultar a SIAC la informaci�n del detalle del art�culo (n�mero de chasis, serie, modelo, marca) del Pedido (l�neas del Pedido), para t�tulos de propiedad.",
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
