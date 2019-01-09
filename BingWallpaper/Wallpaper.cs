using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace BingWallpaper
{
    public abstract class Wallpaper: IWallpaper
    {
        public abstract string Host { get; }


        public async Task SaveAsync(string path)
        {
            await Task.Yield();
            Save(path);
        }
        

        public virtual void Save(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, "wallpaper");
            }
            
            var html = GetHtml();
            var imageUrl = GetImageUrl(html);
            var fileName = GetFileNameFromUrl(imageUrl);
            var bytes = GetUtf8Bytes(imageUrl);
            var path = Path.Combine(directory, fileName);
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            SaveTo(path, bytes);
        }

        protected string GetHtml()
        {
            var util = new HttpClientUtil();
            return util.SendAsync(Host).Result.ReadAsString();
        }

        protected abstract string GetImageUrl(string html);

        protected virtual string GetFileNameFromUrl(string imageUrl)
        {
            return imageUrl.Substring(imageUrl.LastIndexOf("/") + 1);
        }

        protected byte[] GetUtf8Bytes(string url)
        {
            var util = new HttpClientUtil();
            return util.SendAsync(url).Result.ReadAsUTF8Bytes();
        }

        protected void SaveTo(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }
        
        protected string GetActualUrl(string host, string url)
        {
            // url 包含协议头，是绝对路径
            if (url.StartsWith("http"))
            {
                return url;
            }
            
            var uri = new Uri(host);
            if (url.StartsWith("/"))
            {
                // url 以 / 为开始，说明是以网站的根路径开始的相对路径
                return uri.OriginalString.Substring(0, uri.OriginalString.IndexOf(uri.Host) + uri.Host.Length) + url;
            }
            // url 未以 / 开始，是以当前路径的上级目录下的相对路径
            var absolutePath = uri.OriginalString.Substring(0, uri.OriginalString.IndexOf(uri.Host) + uri.Host.Length) + uri.AbsolutePath;
            return absolutePath.Substring(0, absolutePath.LastIndexOf("/") + 1) + url;
        }
    }
}