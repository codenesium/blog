using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BenchmarkApiRequests
{
    public class BenchmarkMiddleware
    {
        private readonly RequestDelegate next;

        public BenchmarkMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            context.Response.OnStarting(() =>
            {
                stopwatch.Stop();
                context.Response.Headers.Add("x-time-elapsed", stopwatch.Elapsed.ToString());
                return Task.CompletedTask;
            });
            await this.next(context);

        }
    }
    public static class BenchmarkMiddlewareExtensions
    {
        public static IApplicationBuilder UseBenchmarkMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BenchmarkMiddleware>();
        }
    }
}
