using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FileUploadServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(IsFileUpload);
            app.Run(Main);
        }

        private async Task IsFileUpload(HttpContext context, Func<Task> next)
        {
            if (IsMultipartContentType(context.Request.ContentType))
            {
                await next.Invoke();
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Not a file upload.");
            }
        }

        private async Task Main(HttpContext context)
        {
            string directoryToUplaod = Configuration.GetValue<string>("directory");

            var boundary = context.Request.GetMultipartBoundary();

            var reader = new MultipartReader(boundary, context.Request.Body);

            MultipartSection section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var dispositionHeader = section.GetContentDispositionHeader();

                if (dispositionHeader.IsFileDisposition())
                {
                    var fileSection = section.AsFileSection();

                    if (!Directory.Exists(directoryToUplaod))
                    {
                        Directory.CreateDirectory(directoryToUplaod);
                    }

                    string path = Path.Combine(directoryToUplaod, fileSection.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await fileSection.FileStream.CopyToAsync(stream);
                    }
                }

                section = await reader.ReadNextSectionAsync();
            }

            await context.Response.WriteAsync("Success.");
        }

        private static bool IsMultipartContentType(string contentType)
        {
            return
                !string.IsNullOrEmpty(contentType) &&
                contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
