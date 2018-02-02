using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algraph.Infrastructure.Web.Http.Results
{
    /// <summary>
    /// 表示一个返回 <see cref="Http"/> 请求的操作结果。
    /// </summary>
    public class HttpResult 
    {
        /// <summary>
        /// 获取或设置一个 <code>Boolean</code> 值，该值表示Http请求的结果。
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 获取或设置消息。
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// 表示一个返回 <see cref="Http"/> 请求的操作结果。
    /// </summary>
    /// <typeparam name="T">请求返回的附加信息。</typeparam>
    public class HttpResult<T> : HttpResult
    {
        /// <summary>
        /// 获取或设置附加信息。
        /// </summary>
        public T Data { get; set; }
    }
}
