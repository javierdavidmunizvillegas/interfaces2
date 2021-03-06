/*
 Objetivo: Valida los campos obligatorios de las clases del Controlador.
 Archivo: ValidadorRequest001.cs
 Versión: 1.0
 Creación: 09/03/2022
 Autor: Solange Moncada
*/

using DCAJ017.api.Models._001.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ017.api.Infraestructure.Configuration
{
    public class ValidadorRequest001 : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        private static RegistroLog Logger = new RegistroLog();
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            APDCAJ017001MessageResponse respuesta = null;

            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                var errors = modelState.Select(x => x.Value.Errors)
                       .Where(y => y.Count > 0)
                       .ToList();

                if (errors != null)
                {
                    respuesta = new APDCAJ017001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();

                    /*Agrega al errorlist todos los campos obligatorios que no ha sido llenados*/
                    foreach (var det in errors)
                    {                       
                        respuesta.ErrorList.Add(det.Select(x => x.ErrorMessage).FirstOrDefault());  
                    }
                    context.Result = new BadRequestObjectResult(respuesta);
                }
                var result = JsonConvert.SerializeObject(context.Result);
                Logger.FileLogger("APDCAJ017001", "VALIDADOR REQUEST: " + result);

                return;
            }
            base.OnActionExecuting(context);
        }
    }

}
