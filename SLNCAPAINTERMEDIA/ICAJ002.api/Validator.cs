using ICAJ002.api.Infraestructura.Configuracion;
using ICAJ002.api.Models._001.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ002.api
{
    public class ValidationFilter001Attribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        private static RegistroLog Logger = new RegistroLog();
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            APICAJ002001MessageResponse respuesta = null;

            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                var errors = modelState.Select(x => x.Value.Errors)
                       .Where(y => y.Count > 0)
                       .ToList();

                if (errors != null)
                {
                    respuesta = new APICAJ002001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    int i = 0;
                    foreach (var det in errors)
                    {
                        respuesta.ErrorList.Add("ICAJ002:E000|" + det.Select(x => x.ErrorMessage).FirstOrDefault());
                        Logger.FileLogger("APICAJ002001", $"VALIDA CAMPOS: {respuesta.ErrorList[i]} ");
                        i++;
                    }
                    context.Result = new BadRequestObjectResult(respuesta);
                }

                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
