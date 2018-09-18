using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BingWallpaper.Bing;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace BingWallpaper
{
    class Program
    {
        static void Main(string[] args)
        {
            var cnBing = new CnBingWallpaper();
            
            var enBing = new EnBingWallpaper();
            cnBing.Save();
            enBing.Save();
            
//            var task1 = cnBing.SaveAsync();
//            var task2 = enBing.SaveAsync();

            Console.WriteLine("waiting for executing");
//            Task.WaitAll(task1, task2);
            Console.WriteLine("main thread finished");
        }
    }
}