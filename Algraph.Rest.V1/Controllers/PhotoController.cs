using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Algraph.Rest.V1.Controllers
{
    /// <summary>
    /// 表示照片的控制器类。
    /// </summary>
    [RoutePrefix("api/v1/photo")]
    public class PhotoController : ApiController
    {
        /// <summary>
        /// 根据分组id获取分组信息。
        /// </summary>
        /// <param name="id">分组id。</param>
        /// <returns>返回分组信息。</returns>
        [Authorize]
        [HttpGet]
        public string Get(long id)
        {
            return "test";
        }

        [Route("abc", Name = "abc")]
        public string xx()
        {
            return "abc";
        }
    }
}
