using ICRE007.api.Infraestructura.Servicios;
using ICRE007.api.Models._001.Request;
using ICRE007.api.Models._001.Response;
using ICRE007.api.Models._002.Request;
using ICRE007.api.Models._002.Response;
using ICRE007.api.Models.Homologa;
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

namespace ICRE007.api
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
            services.AddScoped(typeof(IManejadorRequestQueue<APICRE007001MessageRequest>), typeof(ManejadorRequestQueue<APICRE007001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponseQueue2<APICRE007001MessageResponse>), typeof(ManejadorResponseQueue2<APICRE007001MessageResponse>));
            services.AddScoped(typeof(IManejadorRequestQueue<APICRE007002MessageRequest>), typeof(ManejadorRequestQueue<APICRE007002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponseQueue2<APICRE007002MessageResponse>), typeof(ManejadorResponseQueue2<APICRE007002MessageResponse>));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ICRE007.api",
                    Description = "Consulta de Grupos de clientes y Perfiles de asiento contable de clientes en Dynamics 365FO, los cuales deberán ser consumidos por el cliente desde la capa intermedia",
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
                   //   pattern: "{controller=Home}/{action=Index}/{id?}");
                        pattern: "/{action=Index}/{id?}");
        });
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "Consulta de grupos de clientes y perfiles de contabilización");
               // options.RoutePrefix = string.Empty;
            });
        }
    }
}
