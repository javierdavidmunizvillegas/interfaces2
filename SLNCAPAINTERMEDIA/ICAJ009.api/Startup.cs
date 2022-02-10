using ICAJ009.api.Infraestructura.Servicios;
using ICAJ009.api.Models._001.Request;
using ICAJ009.api.Models._001.Response;
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
using ICAJ009.api.Models.Homologacion;

namespace ICAJ009.api
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
            services.AddScoped<ValidationFilter001Attribute>();
            services.AddControllers();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddScoped(typeof(IHomologacionService<ResponseHomologa>), typeof(HomologacionService<ResponseHomologa>));
            services.AddScoped(typeof(IManejadorRequestQueue<APICAJ009001MessageRequest>), typeof(ManejadorRequestQueue<APICAJ009001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponseQueue2<APICAJ009001MessageResponse>), typeof(ManejadorResponseQueue2<APICAJ009001MessageResponse>));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ICAJ009.api",
                    Description = "Emisión de una Nota de crédito de servicios por el valor correspondiente a los documentos promocionales (cuotas gratis, billete comprador, descuentos, etc.), utilizando la funcionalidad Estándar de MSD365FO llamada “Facturas de servicios”.",
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
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "Emisión de una Nota de crédito de servicios");
                
            });
        }
    }
}
