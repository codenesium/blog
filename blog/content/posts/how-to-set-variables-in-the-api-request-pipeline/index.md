+++
date = "2020-06-25"
title = "How to set variables in the API request pipeline"
slug = "how-to-set-variables-in-the-api-request-pipeline"
tags = [
    ".NET"
]
categories = [
    ".NET",
]
+++
The code for this example can be found on [Github](https://github.com/codenesium/blog/code/tree/master/RequestVariableMiddleware)

Often you want to pass variables from claims or headers into your application. Instead of setting these variables in your controller you can add a middleware that stores variables in the HttpContext.Items collection and this collection is accessible inside a controller. To make it cleaner here I added an abstract controller that makes the variables available to the values controller.

### RequestVariablesMiddleware.cs
```
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
```

### Startup.cs
```
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();

    app.UseVariableMiddleware();

    ...
```

### AbstractController.cs
```
using System;
using Microsoft.AspNetCore.Mvc;

namespace RequestVariableMiddleware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class AbstractController : ControllerBase
    {
        public RequestVariables RequestVariables 
        {
            get
            {
                RequestVariables variables = this.HttpContext.Items["Variables"] as RequestVariables;
                if(variables == null)
                {
                    throw new NullReferenceException("Variables not set in the request pipeline.");
                }
                else
                {
                    return variables;
                }
            }
        }
    }
}
```

### ValuesController.cs
```
using Microsoft.AspNetCore.Mvc;

namespace RequestVariableMiddleware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : AbstractController
    {

        public string Get()
        {
            return $"API_KEY={this.RequestVariables.ApiKey}";
        }
    }
}
```