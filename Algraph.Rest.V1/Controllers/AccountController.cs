using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Algraph.Infrastructure.Web.Http.Results;

namespace Algraph.Rest.V1.Controllers
{
    /// <summary>
    /// 表示账户的控制器类。
    /// </summary>
    public class AccountController : ApiController
    {
        public IHttpActionResult Register()
        {
            return new HttpGenericResult
            {
                Result = true,
                Message = "xixi"
            };
        }

        public async Task<IHttpActionResult> Test()
        {
            var task = await Task.Run(() =>
            {
                return new HttpGenericResult<string>(this.Request)
                 {
                     Result = true,
                     Message = "success",
                     Data = "abc"
                 };
            });
            return task;
        }

        public HttpResult test1()
        {
            return new HttpResult
            {
                Result = true,
                Message = "success"
            };
        }

        public HttpResult<string> test2()
        {
            return new HttpResult<string>
            {
                Result = true,
                Message = "success",
                Data = "abc"
            };
        }
    }
}
