using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ026.api.Models._001.Response
{
    public class APStoreContract
    {
        public string APStoreid { get; set; }//Identificador de tienda
        public string Description { get; set; }//Descripción de tienda
        public string APAddressStore { get; set; }//Dirección de la tienda
        public string City { get; set; }//Ciudad
        public string RegistrationCodeCity { get; set; }//Código de Cantón de Matriculación
        public List<APStoreDetailsResponseContract> NumberSequenceGroupList { get; set; }//Lista de Grupo de Secuencias de series para tienda


    }
}
