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
