using System;
using System.Collections.Generic;
using System.Text;

namespace DSAC003001.Models._001.Response
{
    class APTechnicalReportE
    {
        public string workOrderNumber { get; set; }//Número de Orden de Trabajo
        public string assetNumber { get; set; }//Número de Activo
        public string itemCode { get; set; }//Código de Artículo
        public string serialNumber { get; set; }//Número de Serie
        public string reportType { get; set; }//Tipo de Informe
        public decimal discount { get; set; }//Descuento
    }
}
