using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileUploadServer
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Parameters to use:" + Environment.NewLine +
                    @"directory - directory=c:\files");
                Environment.Exit(0);
            }

            ValidateDirectory(args);
                       
            var host = WebHost.CreateDefaultBuilder(args)
                        .UseKestrel()
                        .UseStartup<Startup>()
                        .Build();

            host.Run();
        }
        
        private static void ValidateDirectory(IEnumerable<string> args)
        {
            string direcotryArg = args.FirstOrDefault(x => x.StartsWith("directory="));

            if (string.IsNullOrEmpty(direcotryArg))
            {
                Console.WriteLine("Parameters directory is required.");
                Environment.Exit(1);
            }

            string directory = direcotryArg.Replace("directory=","");

            if (!Directory.Exists(directory))
            {
                Console.WriteLine("Directory doesn't exist.");
                Environment.Exit(1);
            }
        }
    }
}
