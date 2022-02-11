﻿using ICRE008.api.Models._001.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE008.api.Infraestructure.Configuration
{
    public class ValidadorRequest : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
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
                APICRE009001MessageResponse respuesta = null;

                var modelState = context.ModelState;
                if (!modelState.IsValid)
                {
                    var errors = modelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

                    if (errors != null)
                    {
                        respuesta = new APICRE009001MessageResponse();
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
                    Logger.FileLogger("APICRE009001", "VALIDADOR REQUEST: " + result);

                    return;
                }
                base.OnActionExecuting(context);
            }
        }

    }
}
