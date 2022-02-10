using ISAC017.api.Infraestructura.Servicios;
using ISAC017.api.Infraestructure.Configuration;
using ISAC017.api.Infraestructure.Services;
using ISAC017.api.Models._001.Request;
using ISAC017.api.Models._001.Response;
using ISAC017.api.Models._002.Request;
using ISAC017.api.Models._002.Response;
using ISAC017.api.Models._004.Request;
using ISAC017.api.Models._004.Response;
using ISAC017.api.Models.ResponseHomologacion;
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

namespace ISAC017.api
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
            services.AddScoped<ValidationFilter004Attribute>();
            services.AddControllers();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddScoped(typeof(IManejadorHomologacion<ResponseHomologacion>), typeof(ManejadorHomologacion<ResponseHomologacion>));

            services.AddScoped(typeof(IManejadorRequest<APISAC017001MessageRequest>), typeof(ManejadorRequest<APISAC017001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APISAC017001MessageResponse>), typeof(ManejadorResponse<APISAC017001MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APISAC017002MessageRequest>), typeof(ManejadorRequest<APISAC017002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APISAC017002MessageResponse>), typeof(ManejadorResponse<APISAC017002MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APISAC017004MessageRequest>), typeof(ManejadorRequest<APISAC017004MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APISAC017004MessageResponse>), typeof(ManejadorResponse<APISAC017004MessageResponse>));

            

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Interfaz Documento ISAC017",
                Description = "Interfaz de creaci�n de orden devuelta, actualizaci�n de c�digo de disposici�n y actualizaci�n de estados (envi� de informaci�n a SIAC)",
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
