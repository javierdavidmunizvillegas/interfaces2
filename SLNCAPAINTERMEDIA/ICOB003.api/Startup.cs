using ICOB003.api.Infraestructura.Servicios;
using ICOB003.api.Models._001.Request;
using ICOB003.api.Models._001.Response;
using ICOB003.api.Models._002.Request;
using ICOB003.api.Models._002.Response;
using ICOB003.api.Models._003.Request;
using ICOB003.api.Models._003.Response;
using ICOB003.api.Models.Homologa;
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

namespace ICOB003.api
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
            services.AddScoped<ValidationFilter003Attribute>();
            services.AddControllers();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddScoped(typeof(IHomologacionService<ResponseHomologa>), typeof(HomologacionService<ResponseHomologa>));
            services.AddScoped(typeof(IManejadorRequestQueue<APICOB003003MessageRequest>), typeof(ManejadorRequestQueue<APICOB003003MessageRequest>));
            services.AddScoped(typeof(IManejadorResponseQueue2<APICOB003003MessageResponse>), typeof(ManejadorResponseQueue2<APICOB003003MessageResponse>));

            services.AddScoped(typeof(IManejadorRequestQueue<APICOB003002MessageRequest>), typeof(ManejadorRequestQueue<APICOB003002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponseQueue2<APICOB003002MessageResponse>), typeof(ManejadorResponseQueue2<APICOB003002MessageResponse>));

            //
            services.AddScoped(typeof(IManejadorRequestQueue<APICOB003001MessageRequest>), typeof(ManejadorRequestQueue<APICOB003001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponseQueue2<APICOB003001MessageResponse>), typeof(ManejadorResponseQueue2<APICOB003001MessageResponse>));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ICOB003.api",
                    Description = "Creación de Diarios contables para la realización de las Proviciones de Interés de Postergación y Negociación",
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
