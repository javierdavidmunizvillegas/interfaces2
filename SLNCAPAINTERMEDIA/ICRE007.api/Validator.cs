using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ICRE007.api.Infraestructura.Configuracion;
using ICRE007.api.Models._001.Response;
using ICRE007.api.Models._002.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace ICRE007.api
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
            APICRE007001MessageResponse respuesta = null;
            
            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                var errors = modelState.Select(x => x.Value.Errors)
                       .Where(y => y.Count > 0)
                       .ToList();

                if (errors != null) 
                {
                    respuesta = new APICRE007001MessageResponse();
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
                Logger.FileLogger("APICRE007001", "VALIDADOR REQUEST: " + result);

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
            APICRE007002MessageResponse respuesta = null;

            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                var errors = modelState.Select(x => x.Value.Errors)
                       .Where(y => y.Count > 0)
                       .ToList();

                if (errors != null)
                {
                    respuesta = new APICRE007002MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    int i = 0;
                    foreach (var det in errors)
                    {
                        respuesta.ErrorList.Add("ICRE007:E000|" + det.Select(x => x.ErrorMessage).FirstOrDefault());
                        Logger.FileLogger("APICRE007002", $"VALIDA CAMPOS: {respuesta.ErrorList[i]} ");

                    }

                    context.Result = new BadRequestObjectResult(respuesta);
                }
                return;
            }
            base.OnActionExecuting(context);
        }
    }

}
