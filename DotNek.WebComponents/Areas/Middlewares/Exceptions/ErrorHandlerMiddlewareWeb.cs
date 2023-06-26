using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DotNek.WebComponents.Areas.Middlewares.Exceptions
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;

        private readonly IWebHostEnvironment webHostEnvironment;

        public ErrorHandlerMiddleware(RequestDelegate next, IWebHostEnvironment webHostEnvironment)
        {
            this.next = next;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case AppException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var directoryPath = System.IO.Path.Combine(webHostEnvironment.WebRootPath, "Files");
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var filePath = Path.Combine(directoryPath, "log.csv");
                if (!File.Exists(filePath))
                {
                    var headerString = new StringBuilder();
                    headerString.Append("Date,");
                    headerString.Append("Message,");
                    headerString.Append("File Name,");
                    headerString.Append("Line Number");

                    File.AppendAllText(filePath, headerString + "\r\n");
                }

                var st = new StackTrace(error, true);
                var frames = st.GetFrames();
                var traceString = new StringBuilder();
                traceString.Append(DateTime.Now.ToString("s") + ",");
                traceString.Append("\"" + error.Message.Replace("\n"," ").Replace("\r"," ") + "\",");
                foreach (var frame in frames)
                {
                    if(frame.GetFileLineNumber()<1)
                        continue;
                    traceString.Append(frame.GetFileName() + ",");
                    traceString.Append(frame.GetFileLineNumber());
                    break;
                }
                File.AppendAllText(filePath, traceString + "\r\n");

            }
        }

    }
}