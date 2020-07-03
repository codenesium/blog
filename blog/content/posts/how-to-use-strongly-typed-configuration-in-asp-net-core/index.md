+++
date = "2020-06-19"
title = "How to use strongly typed configuration in ASP.NET Core"
slug = "how-to-use-strongly-typed-configuration-in-asp-net-core"
tags = [
    ".NET"
]
categories = [
    ".NET",
]
+++

The code for this example can be found on [Github](https://github.com/codenesium/blog/tree/master/code/StronglyTypedConfiguration)


.NET Core makes it easy to use a strongly typed object for configuration and inject that object into your classes using the options pattern. This is a lot cleaner than accessing the configuration using ["SectionName"] syntax.  


### This appSettings.json
```
{
  "Settings": {
    "AdminEmail": "admin@codenesium.com",
    "JwtSettings": {
      "JwtSigningKey": "test"
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### Can be represented using these classes
```
namespace StronglyTypedConfiguration
{
    public class ApiSettings
    {
        public string AdminEmail { get; set; }
        public JwtSettings JwtSettings { get; set; }
    }

    public class JwtSettings
    {
        public string SigningKey { get; set; }
    }
}
```


### Configure the settings object to be injected in Startup.cs
```
public void ConfigureServices(IServiceCollection services)
{
    services.Configure<ApiSettings>(this.Configuration.GetSection("Settings"));

    // if you need access to the settings in ConfigureServices you can access it like this.
    ApiSettings settings = new ApiSettings();
    this.Configuration.Bind(settings);
    System.Diagnostics.Debug.Print(settings.AdminEmail);
    ...
```

### Using the settings object inside a controller
Pay attention that you have to inject your object as an IOption object and access Value on that object.

```
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace StronglyTypedConfiguration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : ControllerBase
    {

        private readonly ApiSettings apiSettings;
        private readonly ILogger<SettingsController> logger;

        public SettingsController(IOptions<ApiSettings> apiSettings, ILogger<SettingsController> logger)
        {
            this.apiSettings = apiSettings.Value;
            this.logger = logger;
        }
       
        [HttpGet]
        public async Task <ActionResult<string>> GetAdminEmail()
        {
            return this.apiSettings.AdminEmail;
        }
    }
}
```