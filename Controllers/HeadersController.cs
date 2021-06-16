using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace GraphApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HeadersController : ControllerBase
    {
        private readonly ILogger<HeadersController> _logger;
        private readonly IConfiguration _configuration;

        public HeadersController(ILogger<HeadersController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public Microsoft.AspNetCore.Http.IHeaderDictionary Get()
        {

            Console.WriteLine(HttpContext.Request.Method);
            
            foreach(String key in Request.Headers.Keys)
            {
                Console.WriteLine("Header: " + key + " - Value: " + Request.Headers[key]);
            }

            return Request.Headers;
        }

    }
}