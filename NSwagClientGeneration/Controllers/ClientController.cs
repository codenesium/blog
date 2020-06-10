using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.TypeScript;

namespace NSwagClientGeneration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        [AllowAnonymous]
        [HttpGet]
        [Route("typescript/angular")]
        public async Task<string> AngularTypeScript()
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
                ConfigurationClass = "ApiClientConfig"
            };

            TypeScriptClientGenerator generator = new TypeScriptClientGenerator(document, settings);
            string generatedCode = generator.GenerateFile();
            return generatedCode;
        }

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
    }
}
