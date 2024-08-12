using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CRUD_Operations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOptionsMonitor<AttachmentOptions> _attachments;

        // [IOptions] => registered as singleton (load just once the program start)
        // [IOptionsSnapshot] => registered as scoped (load with every request)
        // [IOptionsMonitor]  => registered as singleton ,but (load with every edition ocure in the code)
        public ConfigController(IConfiguration configuration,IOptionsMonitor<AttachmentOptions> attachments )
        {
            _configuration = configuration;
            _attachments = attachments;
            var value = _attachments.CurrentValue;
        }

        [HttpGet]
        [Route("")]
        public ActionResult GetConfig()
        {
            Thread.Sleep(10000);
            var config = new
            {
                AllowedHosts = _configuration["AllowedHosts"],
                DefaultConnection = _configuration["ConnectionStrings:DefaultConnection"],
                //or use the following way it just for connection string
                //DefaultConnection = _configuration.GetConnectionString("DefaultConnection")
                DefaultLogLevel = _configuration["Logging:LogLevel:Default"],
                TestKey = _configuration["TestKey"],
                SigningKey = _configuration["SigningKey"],
                AttachmentOptions = _attachments.CurrentValue
            };
            return Ok(config);
        }
    }
}
