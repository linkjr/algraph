using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Algraph.Rest.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "test1", "test2" };
        }

        [HttpPost]
        public IEnumerable<string> Get([FromBody]Model model)
        {
            return new string[] { Request.Headers.UserAgent.ToString() };
        }

        public class Model
        {
            public string Key { get; set; }
        }
    }
}
