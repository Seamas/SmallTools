using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace BingWallpaper
{
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// 以String类型读取结果
        /// 如果发生Encoding不支持的情况，需要自行注册Encoding类型
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        public static string ReadAsString(this HttpResponseMessage responseMessage)
        {
            var isGzipCompress = responseMessage.Content.Headers.ContentEncoding.Any(item =>
                item.Contains("gzip", StringComparison.CurrentCultureIgnoreCase));

            if (!isGzipCompress)
            {
                return responseMessage.Content.ReadAsStringAsync().Result;
            }

            var bytes = responseMessage.Content.ReadAsByteArrayAsync().Result;
            var resultBytes = Decompress(bytes);
            var charset = responseMessage.Content.Headers.ContentType.CharSet;
            var encoding = Encoding.GetEncoding(charset);

            return encoding.GetString(resultBytes);
        }

        /// <summary>
        /// 以UTF-8字节读取结果
        /// 如果发生Encoding不支持的情况，需要自行注册Encoding类型
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        public static byte[] ReadAsUTF8Bytes(this HttpResponseMessage responseMessage)
        {
            var isGzipCompress = responseMessage.Content.Headers.ContentEncoding.Any(item =>
                item.Contains("gzip", StringComparison.CurrentCultureIgnoreCase));

            if (!isGzipCompress)
            {
                return responseMessage.Content.ReadAsByteArrayAsync().Result;
            }
            
            var bytes = responseMessage.Content.ReadAsByteArrayAsync().Result;
            var resultBytes = Decompress(bytes);
            var charset = responseMessage.Content.Headers.ContentType.CharSet;
            var encoding = Encoding.GetEncoding(charset);

            return Encoding.UTF8.GetBytes(encoding.GetString(resultBytes));
        }

        /// <summary>
        /// gzip解压内容
        /// </summary>
        /// <param name="gzipData"></param>
        /// <returns></returns>
        private static byte[] Decompress(byte[] gzipData)
        {
            var memoryStream = new MemoryStream(gzipData);
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                var outMemoryStream = new MemoryStream();
            
                var block = new byte[1024];
                for (;;)
                {
                    var bytesRead = gzipStream.Read(block, 0, block.Length);
                
                    if (bytesRead <= 0)
                        break;
                    outMemoryStream.Write(block, 0, bytesRead);

                }

                return outMemoryStream.ToArray();
            }
        }
    }
}