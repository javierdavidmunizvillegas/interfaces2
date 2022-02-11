using IPRO012.api.Infraestructura.Servicios;
using IPRO012.api.Infraestructure.Configuration;
using IPRO012.api.Infraestructure.Services;
using IPRO012.api.Models.ResponseHomologacion;
using IPRO012.Models;
using IPRO012.Models._001;
using IPRO012.Models._002;
using IPRO012.Models._003;
using IPRO012.Models._004;
using IPRO012.Models._005;
using IPRO012.Models._006;
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

namespace IPRO012.api
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

            services.AddScoped(typeof(IManejadorRequest<APIPRO012001MessageRequest>), typeof(ManejadorRequest<APIPRO012001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO012001MessageResponse>), typeof(ManejadorResponse<APIPRO012001MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APIPRO012002MessageRequest>), typeof(ManejadorRequest<APIPRO012002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO012002MessageResponse>), typeof(ManejadorResponse<APIPRO012002MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APIPRO012003MessageRequest>), typeof(ManejadorRequest<APIPRO012003MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO012003MessageResponse>), typeof(ManejadorResponse<APIPRO012003MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APIPRO012004MessageRequest>), typeof(ManejadorRequest<APIPRO012004MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO012004MessageResponse>), typeof(ManejadorResponse<APIPRO012004MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APIPRO012005MessageRequest>), typeof(ManejadorRequest<APIPRO012005MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO012005MessageResponse>), typeof(ManejadorResponse<APIPRO012005MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APIPRO012006MessageRequest>), typeof(ManejadorRequest<APIPRO012006MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APIPRO012006MessageResponse>), typeof(ManejadorResponse<APIPRO012006MessageResponse>));

            //services.AddScoped(typeof(IManejadorRequest<APIPRO012007MessageRequest>), typeof(ManejadorRequest<APIPRO012007MessageRequest>));
            //services.AddScoped(typeof(IManejadorResponse<APIPRO012007MessageResponse>), typeof(ManejadorResponse<APIPRO012007MessageResponse>));

            services.AddScoped<ValidadorRequest001>();
            services.AddScoped<ValidadorRequest002>();
            services.AddScoped<ValidadorRequest003>();
            services.AddScoped<ValidadorRequest004>();
            services.AddScoped<ValidadorRequest005>();
            services.AddScoped<ValidadorRequest006>();
            //services.AddScoped<ValidadorRequest007>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Interfaz Documento IPRO012",
                Description = "Interfaz Envío de Acuerdo de Concesión de Comercialización vigente.",
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
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "Swagger Demo API");
            });
        }
    }
}
