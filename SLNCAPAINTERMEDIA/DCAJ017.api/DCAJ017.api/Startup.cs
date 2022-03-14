/*
 Objetivo: Está encargado de definir los archivos de configuración (en formato JSON mejor que XML) e indicar las modalidades de tratamiento de consultas HTTP.
 Archivo: Startup.cs
 Versión: 1.0
 Creación: 07/03/2022
 Autor: Solange Moncada
*/

using DCAJ017.api.Infraestructura.Servicios;
using DCAJ017.api.Infraestructure.Configuration;
using DCAJ017.api.Infraestructure.Services;
using DCAJ017.api.Models._001.Request;
using DCAJ017.api.Models._001.Response;
using DCAJ017.api.Models._002.Request;
using DCAJ017.api.Models._002.Response;
using DCAJ017.api.Models._003.Request;
using DCAJ017.api.Models._003.Response;
using DCAJ017.api.Models.ResponseHomologacion;
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

namespace DCAJ017.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        /*
          Descripción: En este método se hace el llamado a los controladores 1, 2, 3, homologación y swagger.
          Parámetros de entrada: No aplica
          Parámetros de salida: No aplica
          Último cambio: 07/03/2022
          Autor de último cambio: Solange Moncada
        */
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped(typeof(IManejadorHomologacion<ResponseHomologacion>), typeof(ManejadorHomologacion<ResponseHomologacion>));
            
            services.AddScoped(typeof(IManejadorRequest<APDCAJ017001MessageRequest>), typeof(ManejadorRequest<APDCAJ017001MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APDCAJ017001MessageResponse>), typeof(ManejadorResponse<APDCAJ017001MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APDCAJ017002MessageRequest>), typeof(ManejadorRequest<APDCAJ017002MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APDCAJ017002MessageResponse>), typeof(ManejadorResponse<APDCAJ017002MessageResponse>));

            services.AddScoped(typeof(IManejadorRequest<APDCAJ017003MessageRequest>), typeof(ManejadorRequest<APDCAJ017003MessageRequest>));
            services.AddScoped(typeof(IManejadorResponse<APDCAJ017003MessageResponse>), typeof(ManejadorResponse<APDCAJ017003MessageResponse>));

            services.AddScoped<ValidadorRequest001>();
            services.AddScoped<ValidadorRequest002>();
            services.AddScoped<ValidadorRequest003>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Interfaz Documento DSAC017",
                Description = "Interfaz para Preparación de depósitos de cheques, confirmación de depósitos de cheques y anulaciones de cheques en la gestión de cheques.",
                Version = "v1"
            });
            });
        }

        /*
          Descripción: Método que se ejecutará justo al iniciar la aplicación. Se encuentra configurada la ruta de Swagger.
          Parámetros de entrada: No aplica
          Parámetros de salida: No aplica
          Último cambio: 07/03/2022
          Autor de último cambio: Solange Moncada
        */
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            /*Valores de configuración para Swagger*/
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
