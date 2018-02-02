using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Algraph.Rest.V1.Controllers
{
    public class GroupController : ApiController
    {
        public void Create()
        {

        }

        /// <summary>
        /// 根据分组id获取分组信息。
        /// </summary>
        /// <param name="id">分组id。</param>
        /// <returns>返回分组信息。</returns>
        [HttpGet]
        public string Get(long id)
        {
            return "v1 test";
        }

        public IEnumerable<string> Get()
        {
            return new string[] { "v1 test1", "v1 test2" };
        }
    }
}
