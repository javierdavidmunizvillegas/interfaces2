using IPRO005.api.Infraestructura.Servicios;
using IPRO005.api.Infraestructure.Configuration;
using IPRO005.api.Infraestructure.Services;
using IPRO005.api.Models._001.Request;
using IPRO005.api.Models._001.Response;
using IPRO005.api.Models._002.Request;
using IPRO005.api.Models._002.Response;
using IPRO005.api.Models._003.Request;
using IPRO005.api.Models._003.Response;
using IPRO005.api.Models._004.Request;
using IPRO005.api.Models._004.Response;
using IPRO005.api.Models._005.Request;
using IPRO005.api.Models._005.Response;
using IPRO005.api.Models._006.Request;
using IPRO005.api.Models._006.Response;
using IPRO005.api.Models._007.Request;
using IPRO005.api.Models._007.Response;
using IVTA005.api.Models.ResponseHomologacion;
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

namespace IPRO005.api
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
            services.AddScoped(typeof(IManejadorRequest<APIPRO005001MessageRequest>), typeof(ManejadorRequest<APIPRO005001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO005001MessageResponse>), typeof(ManejadorResponse<APIPRO005001MessageResponse>));
            services.AddScoped(typeof(IManejadorRequest<APIPRO005002MessageRequest>), typeof(ManejadorRequest<APIPRO005002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO005002MessageResponse>), typeof(ManejadorResponse<APIPRO005002MessageResponse>));
            services.AddScoped(typeof(IManejadorRequest<APIPRO005003MessageRequest>), typeof(ManejadorRequest<APIPRO005003MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO005003MessageResponse>), typeof(ManejadorResponse<APIPRO005003MessageResponse>));
            services.AddScoped(typeof(IManejadorRequest<APIPRO005004MessageRequest>), typeof(ManejadorRequest<APIPRO005004MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO005004MessageResponse>), typeof(ManejadorResponse<APIPRO005004MessageResponse>));
            services.AddScoped(typeof(IManejadorRequest<APIPRO005005MessageRequest>), typeof(ManejadorRequest<APIPRO005005MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO005005MessageResponse>), typeof(ManejadorResponse<APIPRO005005MessageResponse>));
            services.AddScoped(typeof(IManejadorRequest<APIPRO005006MessageRequest>), typeof(ManejadorRequest<APIPRO005006MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO005006MessageResponse>), typeof(ManejadorResponse<APIPRO005006MessageResponse>));
            services.AddScoped(typeof(IManejadorRequest<APIPRO005007MessageRequest>), typeof(ManejadorRequest<APIPRO005007MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO005007MessageResponse>), typeof(ManejadorResponse<APIPRO005007MessageResponse>));

            services.AddScoped<ValidadorRequest01>();
            services.AddScoped<ValidadorRequest02>();
            services.AddScoped<ValidadorRequest03>();
            services.AddScoped<ValidadorRequest04>();
            services.AddScoped<ValidadorRequest05>();
            services.AddScoped<ValidadorRequest06>();
            services.AddScoped<ValidadorRequest07>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Interfaz Documento IPRO005",
                    Description = "Interfaz Entre MD365FO-R para Mapeo de Producto, se realizara las consultas con sistema legado R en Dynamics 365FO, los cuales deberán ser consumidos por el cliente desde la capa intermedia. ",
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
