+++
date = "2020-06-10"
title = "How to generate a C# and TypeScript client for an ASP.NET Core API with NSwag"
slug = "how-to-generate-a-csharp-and-typescript-client-for-an-asp-net-core-api-with-nswag"
tags = [
    ".NET","Swagger","NSwag"
]
categories = [
    ".NET","Swagger"
]
+++

[NSwag](https://github.com/RicoSuter/NSwag) is an awesome library for generating C# and TypeScript clients for Swagger APIs.

A complete example of this code can be found on our [Github](https://github.com/codenesium/blog/tree/master/NSwagClientGeneration)


This is an example of how to add an endpoint to your API to generate an Angular TypeScript client and a C# client. It's possible to use NSwag with Swashbuckle.
These methods retrieve the swagger json from the API and then return a client. You can provide your own class for AuthorizedApiBase and ApiClientConfig and there are numerous configuration options. 


```
Install-Package NSwag.CodeGeneration.TypeScript
```

```
[Route("api/client")]
[ApiController]
[AllowAnonymous]
public class ClientController : Controller
{
    [HttpGet]
    [Route("typescript/angular")]
    public async Task<string> Get()
    {
        string baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
        OpenApiDocument document = await OpenApiDocument.FromUrlAsync($"{baseUrl}/swagger/v1/swagger.json");

        TypeScriptClientGeneratorSettings settings = new TypeScriptClientGeneratorSettings
        {
            ClientBaseClass = "AuthorizedApiBase",
            ClassName = "ClientName",
            Template = TypeScriptTemplate.Angular,
            PromiseType = PromiseType.Promise,
            HttpClass = HttpClass.HttpClient,
            InjectionTokenType = InjectionTokenType.InjectionToken,
            ConfigurationClass ="ApiClientConfig"
        };

        TypeScriptClientGenerator generator = new TypeScriptClientGenerator(document, settings);
        string generatedCode = generator.GenerateFile();
    }
}
```


These classes would reside in your TypeScript project and can be configured however you like. ApiClientConfig.getBearerToken
could return a token from local storage or from wherever you have it stored.
```
export class ApiClientConfig {
  /**
   * Returns a valid value for the Authorization header.
   * Used to dynamically inject the current auth header.
   */
  getBearerToken: () => '';
}

export class AuthorizedApiBase {
  private readonly config: ApiClientConfig;

  protected constructor(config: ApiClientConfig) {
    this.config = config;
  }

  protected transformOptions = (options: RequestInit): Promise<RequestInit> => {
    options.headers = {
      ...options.headers,
      'Authorization': 'Bearer ' + this.config.getBearerToken(),
    };
    return Promise.resolve(options);
  };
  ```


For C#

```
Install-Package NSwag.CodeGeneration.CSharp
```

```
[AllowAnonymous]
[HttpGet]
[Route("csharp")]
public async Task<string> CSharp()
{
    string baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
    OpenApiDocument document = await OpenApiDocument.FromUrlAsync($"{baseUrl}/swagger/v1/swagger.json");

    CSharpClientGeneratorSettings settings = new CSharpClientGeneratorSettings
    {
        ClassName = "MyAPIClient",
        CSharpGeneratorSettings =
        {
            Namespace = "MyNamespace",
        }
    };

    CSharpClientGenerator generator = new CSharpClientGenerator(document, settings);
    string generatedCode = generator.GenerateFile();
    return generatedCode;
}
```