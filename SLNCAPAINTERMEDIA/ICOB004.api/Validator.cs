using ICOB004.api.Infraestructura.Configuracion;
using ICOB004.api.Models._001.Response;
using ICOB004.api.Models._002.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB004.api
{
    public class Validator
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
                APICOB004001MessageResponse respuesta = null;

                var modelState = context.ModelState;
                if (!modelState.IsValid)
                {
                    var errors = modelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

                    if (errors != null)
                    {
                        respuesta = new APICOB004001MessageResponse();
                        respuesta.SessionId = string.Empty;
                        respuesta.StatusId = false;
                        respuesta.ErrorList = new List<string>();

                        foreach (var det in errors)
                        {
                            respuesta.ErrorList.Add(det.Select(x => x.ErrorMessage).FirstOrDefault());

                        }
                        context.Result = new BadRequestObjectResult(respuesta);
                    }
                    var result = JsonConvert.SerializeObject(context.Result);
                    Logger.FileLogger("APICOB004001", "VALIDADOR REQUEST: " + result);

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
                APICOB004002MessageResponse respuesta = null;

                var modelState = context.ModelState;
                if (!modelState.IsValid)
                {
                    var errors = modelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

                    if (errors != null)
                    {
                        respuesta = new APICOB004002MessageResponse();
                        respuesta.SessionId = string.Empty;
                        respuesta.StatusId = false;
                        respuesta.ErrorList = new List<string>();
                        int i = 0;
                        foreach (var det in errors)
                        {
                            respuesta.ErrorList.Add("ICOB004:E000|" + det.Select(x => x.ErrorMessage).FirstOrDefault());
                            Logger.FileLogger("APICOB004002", $"VALIDA CAMPOS: {respuesta.ErrorList[i]} ");

                        }

                        context.Result = new BadRequestObjectResult(respuesta);
                    }
                    return;
                }
                base.OnActionExecuting(context);
            }
        }
    }
}
