﻿using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ICXP003.api.Infraestructure.Services
{
    public static class ValidarEsquemaJSON
    {
        public static bool IsJsonValid<TSchema>(this string value)
         where TSchema : new()
        {
            bool res = true;
            //this is a .net object look for it in msdn
            JavaScriptSerializer ser = new JavaScriptSerializer();
            //first serialize the string to object.
            var obj = ser.Deserialize<TSchema>(value);

            //get all properties of schema object
            var properties = typeof(TSchema).GetProperties();
            //iterate on all properties and test.
            foreach (PropertyInfo info in properties)
            {
                // i went on if null value then json string isnt schema complient but you can do what ever test you like her.
                var valueOfProp = obj.GetType().GetProperty(info.Name).GetValue(obj, null);
                if (valueOfProp == null)
                    res = false;
            }

            return res;
        }

    }
}
