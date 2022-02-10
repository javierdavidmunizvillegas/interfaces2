using ICAJ020.api.Infraestructura.Configuracion;
using ICAJ020.api.Models._001.Response;
using ICAJ020.api.Models._002.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ020.api
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
            APICAJ020001MessageResponse respuesta = null;

            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                var errors = modelState.Select(x => x.Value.Errors)
                       .Where(y => y.Count > 0)
                       .ToList();

                if (errors != null)
                {
                    respuesta = new APICAJ020001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    int i = 0;
                    foreach (var det in errors)
                    {
                        respuesta.ErrorList.Add("ICAJ020:E000|" + det.Select(x => x.ErrorMessage).FirstOrDefault());
                        Logger.FileLogger("APICAJ020001", $"VALIDA CAMPOS: {respuesta.ErrorList[i]} ");
                        i++;
                    }
                    context.Result = new BadRequestObjectResult(respuesta);
                }

                return;
            }
            base.OnActionExecuting(context);
        }
    }
    public class ValidationFilter002Attribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        private static RegistroLog Logger = new RegistroLog();
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            APICAJ020002MessageResponse respuesta = null;

            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                var errors = modelState.Select(x => x.Value.Errors)
                       .Where(y => y.Count > 0)
                       .ToList();

                if (errors != null)
                {
                    respuesta = new APICAJ020002MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    int i = 0;
                    foreach (var det in errors)
                    {
                        respuesta.ErrorList.Add("ICAJ020:E000|"+det.Select(x => x.ErrorMessage).FirstOrDefault());
                        Logger.FileLogger("APICAJ020002", $"VALIDA CAMPOS: {respuesta.ErrorList[i]} ");

                    }

                    context.Result = new BadRequestObjectResult(respuesta);
                }
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
