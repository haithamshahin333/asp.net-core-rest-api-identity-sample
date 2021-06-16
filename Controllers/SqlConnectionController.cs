using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace GraphApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SqlConnectionController : ControllerBase
    {

        private readonly ILogger<SqlConnectionController> _logger;
        private readonly IConfiguration _configuration;

        public SqlConnectionController(ILogger<SqlConnectionController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/sqlconnection")]
        public String GetConnection()
        {
            var userAccessToken = Request.Headers["X-MS-TOKEN-AAD-ACCESS-TOKEN"];

            try
            {

                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("connectionString")))
                {
                    conn.AccessToken = userAccessToken;
                    conn.Open();
                    string query = "SELECT SYSTEM_USER;";
                    SqlCommand command = new SqlCommand(query, conn);
                    return command.ExecuteScalar().ToString();
                }

            }
            catch (Exception e)
            {
                return e.ToString();
            }

        }

    }
}
