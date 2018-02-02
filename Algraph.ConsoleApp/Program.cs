using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Algraph.ConsoleApp
{
    class Program
    {
        private const string apiHost = "http://www.algraph.com/api/";
        private const string restHost = "http://www.algraph.com/rest/";

        static void Main(string[] args)
        {
            var client = new HttpClient();
            //var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiHost + "api/Values/get/1");
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, restHost + "rest/Values/1");

            //The same as
            //client.DefaultRequestHeaders.Add("Authorization", "Basic admin");
            //requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "admin");
            requestMessage.Headers.Add("Authorization", "Basic admin:pwd");
            var task = client.SendAsync(requestMessage, CancellationToken.None)
                .ContinueWith(m =>
                {
                    Console.WriteLine(m.Result.Content.ReadAsStringAsync().Result);
                    Thread.Sleep(3000);
                });
            task.Wait();
        }
    }
}
