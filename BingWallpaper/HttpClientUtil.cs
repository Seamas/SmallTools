using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpaper
{
    public class HttpClientUtil
    {
        private readonly HttpClient _client;
        
        public HttpClientUtil()
        {
            var handler = new HttpClientHandler
            {
                UseCookies = false
            };
            
            _client = new HttpClient(handler);
        }
        
        
        /// <summary>
        /// 异步返回HttpResponseMessage
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="httpMethod">Http请求方式，默认为GET方法</param>
        /// <param name="headers">头部，默认为空</param>
        /// <param name="body">消息体，默认为空</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> SendAsync(string url,
            HttpMethod httpMethod = null,
            IDictionary<string, string> headers = null,
            IDictionary<string, string> body = null)
        {
            var (address, content) = BuildUrlAndContent(url, httpMethod, headers, body);
            
            var message = new HttpRequestMessage(httpMethod ?? HttpMethod.Get, address);
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    message.Headers.Add(header.Key, header.Value);
                }
            }
            
            message.Content = content;
            return _client.SendAsync(message);
        }

        /// <summary>
        /// 根据url, http方法，header,body的参数，构建最终请求的url地址和消息体
        /// 若采用 GET方法，将body中的参数放入url中并进行url编码, 其余方法放入body中
        /// 如果headers中包含json的说明，对body消息体以json的方式提交，否则以form的形式提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="httpMethod"></param>
        /// <param name="headers"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private (string address, HttpContent content) BuildUrlAndContent(string url, 
            HttpMethod httpMethod,
            IDictionary<string, string> headers,
            IDictionary<string, string> body)
        {
            if (httpMethod == HttpMethod.Get)
            {
                if (body != null && body.Count > 0)
                {
                    var stringBuilder = body.Aggregate(new StringBuilder(), 
                        (sb, kvp) => sb.Append($"{(sb.Length > 0 ? "&":"")}{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}") );
                    
                    return (url.Trim() + (url.Trim().EndsWith("?") ? "":"?" )+ stringBuilder, null);
                }
            }

            if (body == null || body.Count <= 0) return (url, null);

            HttpContent content;
            if (headers == null || !headers.Any(item => item.Value.Contains("json")))
            {
                content = new FormUrlEncodedContent(
                    body.Select(item => new KeyValuePair<string, string>(item.Key, WebUtility.UrlEncode(item.Value))));
            }
            else
            {
                var stringBuilder = body.Aggregate(new StringBuilder(),
                    (builder, kvp) => builder.Append($"{(builder.Length > 0 ? "," : "")}")
                        .Append("\"").Append(kvp.Key).Append("\":\"").Append(kvp.Value).Append("\""));
                
                var json = "{"+ stringBuilder +"}";
                content = new StringContent(json);
            }
            
            return (url, content);

        }
    }
}