using Microsoft.AspNetCore.Mvc;

namespace FileSystemWatcherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TesterController : ControllerBase
    {
        private readonly ILogger<TesterController> _logger;

        public TesterController(ILogger<TesterController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Test works!!!";
        }
    }
}