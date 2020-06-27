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
