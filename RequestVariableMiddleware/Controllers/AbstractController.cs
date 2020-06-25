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
    public abstract class AbstractController : ControllerBase
    {
        public RequestVariables RequestVariables 
        {
            get
            {
                return this.HttpContext.Items["Variables"] as RequestVariables;
            }
        } 
    }
}
