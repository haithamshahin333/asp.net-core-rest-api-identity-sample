using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace GraphApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HeadersController : ControllerBase
    {
        private readonly ILogger<HeadersController> _logger;

        public HeadersController(ILogger<HeadersController> logger)
        {
            _logger = logger;
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