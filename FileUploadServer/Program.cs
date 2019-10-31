using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace FileUploadServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = WebHost.CreateDefaultBuilder(args)
                        .UseKestrel()
                        .UseStartup<Startup>()
                        .Build();

            host.Run();
        }
    }
}
