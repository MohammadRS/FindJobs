using DotNek.Common.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FindJobs.WebApi.CustomeMiddleWares
{
    public class UnauthorizedMiddlware
    {
        private readonly RequestDelegate next;

        public UnauthorizedMiddlware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await next(httpContext);
            if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                httpContext.Response.Clear();
                await httpContext.Response.WriteAsJsonAsync<ResultDto>(new ResultDto((int)MessageCodes.Unauthorized));
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class UnauthorizedMiddlwareExtensions
    {
        public static IApplicationBuilder UseUnauthorizedMiddlware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UnauthorizedMiddlware>();
        }
    }
}
