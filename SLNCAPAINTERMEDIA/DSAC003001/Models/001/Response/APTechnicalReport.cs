using System;
using System.Collections.Generic;
using System.Text;

namespace DSAC003001.Models._001.Response
{
    class APTechnicalReport
    {
        public string WorkOrderNumber { get; set; }//Número de Orden de Trabajo
        public string AssetNumber { get; set; }//Número de Activo
        public string ItemCode { get; set; }//Código de Artículo
        public string SerialNumber { get; set; }//Número de Serie
        public string ReportType { get; set; }//Tipo de Informe
        public decimal Discount { get; set; }//Descuento
        public Boolean Suitable { get; set; }//Apto =  true, NoApto = False


    }
}
