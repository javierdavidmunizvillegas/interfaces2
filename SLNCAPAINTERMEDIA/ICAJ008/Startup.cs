using ICAJ008.api;
using ICAJ008.api.Infraestructura.Servicios;
using ICAJ008.api.Models._001.Request;
using ICAJ008.api.Models._001.Response;
using ICAJ008.api.Models.Homologa;
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

namespace ICAJ008
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
            services.AddScoped(typeof(IManejadorRequestQueue<APICAJ008001MessageRequest>), typeof(ManejadorRequestQueue<APICAJ008001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponseQueue2<APICAJ008001MessageResponse>), typeof(ManejadorResponseQueue2<APICAJ008001MessageResponse>));

                      services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ICAJ008.api",
                    Description = "CAJ-008 Interfaz con SIAC Caja para generar desde el MSD365FO el proceso de facturación al momento de la venta.CAJ - 013.Desarrollo de Facturación luego del Despacho:",
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
                        //  pattern: "{controller=Home}/{action=Index}/{id?}");
                        pattern: "{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "proceso de facturación al momento de la venta y el desarrollo de facturación luego del despacho");
            });
        }
    }
}
