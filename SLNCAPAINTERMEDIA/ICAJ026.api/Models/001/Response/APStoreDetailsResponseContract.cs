using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ026.api.Models._001.Response
{
    public class APStoreDetailsResponseContract
    {
        public string NumberSequenceGroup { get; set; }//Número de Secuencia de series para tienda
        public int TypeDocument { get; set; }//APTypeDocumentIVTA004 {0:none, 18:Factura, 4:Nota Credito, 5:Nota Débito, 7:Retención, 6:Remisión, 3:Liquidación, 8:tickect, 99:Internal, 100: Nota de Crédito Interna, 101:Nota de Débito Interna
        public int APBillBuyerCheck { get; set; }
    }
}