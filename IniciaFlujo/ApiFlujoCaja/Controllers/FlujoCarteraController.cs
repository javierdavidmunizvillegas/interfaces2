using ApiModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiFlujoCaja.Controllers
{
    public class FlujoCarteraController : ApiController
    {
        [HttpPost]
        [Route("api/inicioflujoCartera")]
        public IHttpActionResult IniciaFlujo([FromBody] ICAJ008Response jObjectParametros)
        {
            JObject jsonValores = new JObject();
            JArray p_resultadoTemporal = null;

            return Ok("OK");
        }
    }
}
