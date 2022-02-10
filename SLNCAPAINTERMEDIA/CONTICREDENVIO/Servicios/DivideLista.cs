using CONTICREDENVIO.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CONTICREDENVIO.Servicios
{
    public class DivideLista
    {
        public static List<List<Producto>> Split(List<Producto> source, int maxSubItems)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / maxSubItems)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
