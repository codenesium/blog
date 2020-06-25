using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestVariableMiddleware
{
    public class VariableMiddleware
    {
        private readonly RequestDelegate next;

        public VariableMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            RequestVariables variables = new RequestVariables();
            variables.ApiKey = httpContext.Request.Headers["Authorization"];
            httpContext.Items["Variables"] = variables;
            await this.next(httpContext);
        }
    }

    public static class VariableMiddlewareExtensions
    {
        public static IApplicationBuilder UseVariableMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<VariableMiddleware>();
        }
    }
}
