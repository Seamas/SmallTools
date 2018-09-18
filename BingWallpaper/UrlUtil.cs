using System;

namespace BingWallpaper
{
    public class UrlUtil
    {
        /// <summary>
        /// 根据url地址，以及提取的资源src地址，创建资源的最终uri地址
        /// </summary>
        /// <param name="url">当前访问的地址</param>
        /// <param name="src">从当前访问地址中提取出来的src地址</param>
        /// <returns></returns>
        public static string GetActualUrl(string url, string src)
        {
            // src 包含协议头，是绝对路径
            if (src.StartsWith("http"))
            {
                return src;
            }
            
            var uri = new Uri(url);
            if (src.StartsWith("/"))
            {
                // src 以 / 为开始，说明是以网站的根路径开始的相对路径
                return uri.OriginalString.Substring(0, uri.OriginalString.IndexOf(uri.Host) + uri.Host.Length) + src;
            }
            // src 未以 / 开始，是以当前路径的上级目录下的相对路径
            var absolutePath = uri.OriginalString.Substring(0, uri.OriginalString.IndexOf(uri.Host) + uri.Host.Length) + uri.AbsolutePath;
            return absolutePath.Substring(0, absolutePath.LastIndexOf("/") + 1) + src;
        }
    }
}