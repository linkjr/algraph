using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Algraph.Infrastructure.Web.Http.Results
{
    /// <summary>
    /// 表示一个返回 <see cref="Http"/> 请求的操作结果。
    /// </summary>
    public class HttpGenericResult : IHttpActionResult
    {
        /// <summary>
        /// 获取或设置一个 <code>Boolean</code> 值，该值表示Http请求的结果。
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 获取或设置消息。
        /// </summary>
        public string Message { get; set; }

        protected HttpStatusCode StatusCode { get; set; }


        public HttpGenericResult(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            this.StatusCode = statusCode;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var value = new HttpGenericMessage { Result = Result, Message = Message };
            var response = new HttpResponseMessage(this.StatusCode) { Content = new ObjectContent(typeof(HttpGenericMessage), value, new JsonMediaTypeFormatter()), };
            return Task.FromResult(response);
        }
    }
    /// <summary>
    /// 表示一个返回 <see cref="Http"/> 请求的操作结果。
    /// </summary>
    /// <typeparam name="T">请求返回的附加信息。</typeparam>
    public class HttpGenericResult<T> : IHttpActionResult
    {
        /// <summary>
        /// 获取或设置一个 <code>Boolean</code> 值，该值表示Http请求的结果。
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 获取或设置消息。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 获取或设置数据。
        /// </summary>
        public T Data { get; set; }

        protected HttpRequestMessage Request { get; set; }


        public HttpGenericResult(HttpRequestMessage request)
        {
            this.Request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var value = new HttpGenericMessageData<T>
            {
                Result = this.Result,
                Message = this.Message,
                Data = this.Data
            };
            var negotiate = this.Request.GetConfiguration().Services.GetContentNegotiator();
            var request = negotiate.Negotiate(typeof(HttpMessageContent), this.Request, this.Request.GetConfiguration().Formatters);
            var response = new HttpResponseMessage { Content = new ObjectContent<HttpGenericMessageData<T>>(value, request.Formatter, request.MediaType.MediaType) };
            return Task.FromResult(response);
        }
    }

    /// <summary>
    /// 表示 <see cref="Http"/> 请求返回的消息基类。
    /// </summary>
    public class HttpGenericMessage
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
    /// 派生的 <see cref="HttpGenericMessage"/> 类，该类包含 <see cref="Http"/> 请求返回的附加信息。
    /// </summary>
    public class HttpGenericMessageData<TData> : HttpGenericMessage
    {
        /// <summary>
        /// 获取或设置数据。
        /// </summary>
        public TData Data { get; set; }
    }
}
