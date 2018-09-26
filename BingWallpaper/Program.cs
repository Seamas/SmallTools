using System;
using System.Collections.Generic;
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
            var wallpaperTypes = Assembly.GetExecutingAssembly().GetTypes().Where(item =>
                !item.IsAbstract && item.GetInterface(typeof(IWallpaper).FullName) != null).ToList();

            Console.WriteLine("waiting for executing");
            var taskList = new List<Task>();

            foreach (var wallpaperType in wallpaperTypes)
            {
                var wallPaper = (IWallpaper)Activator.CreateInstance(wallpaperType);
                
                taskList.Add(wallPaper.SaveAsync(args.Length == 0 ? string.Empty: args[0]));
            }
                      
            Task.WaitAll(taskList.ToArray());
            Console.WriteLine("main thread finished");
        }
    }
}
