using IPRO007.api.Infraestructura.Servicios;
using IPRO007.api.Models._001.Request;
using IPRO007.api.Models._001.Response;
using IPRO007.api.Models._002.Request;
using IPRO007.api.Models._002.Response;
using IPRO007.api.Models.Homologa;
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


namespace IPRO007.api
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
            services.AddScoped(typeof(IHomologacionService<ResponseHomologa>), typeof(HomologacionService<ResponseHomologa>));
            services.AddScoped(typeof(IManejadorRequestQueue<APIPRO007002MessageRequest>), typeof(ManejadorRequestQueue<APIPRO007002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponseQueue2<APIPRO007002MessageResponse>), typeof(ManejadorResponseQueue2<APIPRO007002MessageResponse>));

            //
            services.AddScoped(typeof(IManejadorRequestQueue<APIPRO007001MessageRequest>), typeof(ManejadorRequestQueue<APIPRO007001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponseQueue2<APIPRO007001MessageResponse>), typeof(ManejadorResponseQueue2<APIPRO007001MessageResponse>));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "IPRO007.api",
                    Description = "Brinda información de la inserción de registros en el formulario de Pronósticos de la demanda",
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
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "Gestión de Cobranza – Retail");
            });
        }
    }
}
