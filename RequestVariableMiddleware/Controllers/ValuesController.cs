using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
