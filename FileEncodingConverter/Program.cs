using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Linq;

namespace FileEncodingConverter
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var result = GetConfiguration();
            
//            Console.WriteLine($"{result.source.EncodingName}, {result.dest.EncodingName}, {result.sourceDirectory}, {result.destDirectory}");

            var directory = new DirectoryInfo(result.sourceDirectory);
            var files = directory.GetFiles()
                .Where(item => string.IsNullOrEmpty(result.filter) || item.Extension.EndsWith(result.filter, StringComparison.CurrentCultureIgnoreCase));
            
            foreach (var fileInfo in files)
            {
                Console.WriteLine($"start convert {fileInfo.FullName}");
                EncodingConvert(result.source, result.dest, fileInfo, result.destDirectory);
            }
            
            Console.WriteLine("finished");
        }

        static (Encoding source, Encoding dest, string sourceDirectory, string destDirectory, string filter) GetConfiguration()
        {
            var _source = Encoding.GetEncoding(ConfigurationManager.AppSettings["source"]);
            var _dest = Encoding.GetEncoding(ConfigurationManager.AppSettings["dest"]);
            var _sourceDirectory = ConfigurationManager.AppSettings["sourceDir"];
            var _destDirectory = ConfigurationManager.AppSettings["destDir"];
            var _filter = ConfigurationManager.AppSettings["filter"];

            return (_source, _dest, _sourceDirectory, _destDirectory, _filter);
        }

        static void EncodingConvert(Encoding source, Encoding dest, FileInfo fileInfo, string destDirectory)
        {
            using (var reader = new StreamReader(fileInfo.FullName, source))
            {
                var path = Path.Combine(destDirectory, fileInfo.Name);
                using (var writer = new StreamWriter(new FileStream(path, FileMode.OpenOrCreate),
                    dest == Encoding.UTF8 ? new UTF8Encoding(false) : dest))
                {
                    writer.AutoFlush = true;
                    var line = reader.ReadLine();
                    writer.WriteLine(line);
                }
            }
        }
    }
}