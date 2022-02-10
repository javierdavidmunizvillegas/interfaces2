using ICOB004.api.Infraestructura.Servicios;
using ICOB004.api.Models._001.Request;
using ICOB004.api.Models._001.Response;
using ICOB004.api.Models._002.Request;
using ICOB004.api.Models._002.Response;
using ICOB004.api.Models.Homologacion;
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
using static ICOB004.api.Validator;

namespace ICOB004.api
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
            services.AddScoped(typeof(IConsumoWebService<ResponseHomologa>), typeof(ConsumoWebService<ResponseHomologa>));
            services.AddScoped(typeof(IManejadorRequestQueue<APICOB004001MessageRequest>), typeof(ManejadorRequestQueue<APICOB004001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponseQueue2<APICOB004001MessageResponse>), typeof(ManejadorResponseQueue2<APICOB004001MessageResponse>));
            services.AddScoped(typeof(IManejadorRequestQueue<APICOB004002MessageRequest>), typeof(ManejadorRequestQueue<APICOB004002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponseQueue2<APICOB004002MessageResponse>), typeof(ManejadorResponseQueue2<APICOB004002MessageResponse>));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ICOB004.api",
                    Description = "Interfaz para el registro de notas de créditos y diarios contables para dar de baja a la deuda anterior y dar de alta a la nueva deuda",
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

                        pattern: "{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "Dar de baja a una deuda antigua y dar de alta a una nueva deuda.");
                //options.RoutePrefix = string.Empty;
            });

        }
    }
}
