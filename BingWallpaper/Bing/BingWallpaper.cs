using System;
using System.Linq;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace BingWallpaper.Bing
{
    public abstract class BingWallpaper: Wallpaper
    {
        const string flag = "g_img={url";
        const string startQuote = "{";
        const string endQuote = "}";
        
        
        protected override string GetImageUrl(string html)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var nodes = htmlDocument.DocumentNode.Descendants("script");
            var script = nodes.FirstOrDefault(item => item.InnerText.Contains(flag, StringComparison.InvariantCultureIgnoreCase)).InnerText;
            var imageIndex = script.IndexOf(flag);
            var startIndex = script.IndexOf(startQuote, imageIndex);
            var endIndex = script.IndexOf(endQuote, startIndex);
            
            var json = script.Substring(startIndex, endIndex - startIndex + 1);
            var gImg = JsonConvert.DeserializeObject<dynamic>(json);

            var imgUrl = (string)gImg.url.ToString();
            return GetActualUrl(Host, imgUrl);
        }

        protected override string GetFileNameFromUrl(string imageUrl)
        {
            var fileName = imageUrl.Substring(imageUrl.LastIndexOf("/") + 1);

            var index = fileName.IndexOf('_');
            var lastIndex = fileName.LastIndexOf('.');

            return fileName.Substring(0, index) + fileName.Substring(lastIndex);
        }

    }
}