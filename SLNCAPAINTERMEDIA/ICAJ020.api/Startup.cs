using ICAJ020.api.Infraestructura.Servicios;
using ICAJ020.api.Models._001.Request;
using ICAJ020.api.Models._001.Response;
using ICAJ020.api.Models._002.Request;
using ICAJ020.api.Models._002.Response;
using ICAJ020.api.Models.Homologacion;
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

namespace ICAJ020.api
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
            services.AddScoped<ValidationFilter002Attribute>();
            services.AddControllers();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddScoped(typeof(IHomologacionService<ResponseHomologa>), typeof(HomologacionService<ResponseHomologa>));
            services.AddScoped(typeof(IManejadorRequestQueue<APICAJ020001MessageRequest>), typeof(ManejadorRequestQueue<APICAJ020001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponseQueue2<APICAJ020001MessageResponse>), typeof(ManejadorResponseQueue2<APICAJ020001MessageResponse>));
            services.AddScoped(typeof(IManejadorRequestQueue<APICAJ020002MessageRequest>), typeof(ManejadorRequestQueue<APICAJ020002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponseQueue2<APICAJ020002MessageResponse>), typeof(ManejadorResponseQueue2<APICAJ020002MessageResponse>));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ICAJ020.api",
                    Description = "Interfaz para realizar reversos de los recaudos (recaudos de pedidos, recaudos de anticipos, recaudos de cartera, recaudos de tercero) y facturas generadas de manera estandar.",
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
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "Interfaz para realizar reversos de los recaudos ");

            });
        }
    }
}
