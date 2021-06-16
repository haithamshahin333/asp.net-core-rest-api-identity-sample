using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;

namespace GraphApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public GraphController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        [Route("/azureidentity")]
        public String GetAzureIdentity()
        {

            // https://docs.microsoft.com/en-us/dotnet/api/overview/azure/identity-readme?view=azure-dotnet
            var credential = new DefaultAzureCredential();
            var token = credential.GetToken(
                new Azure.Core.TokenRequestContext(
                    new[] { "https://graph.microsoft.com/.default" }));

            var accessToken = token.Token;
            return accessToken;
        }

        [HttpGet]
        [Route("/graphclient")]
        public async Task<IActionResult> GetGraphClientAsync()
        {

            /*
             * Harden:
             * 1. Use Managed Identity to get app credentials from key vault
             * 2. Authenticate with Client credentials
             * 3. Query Graph
             */

            // Build a client application.
            // https://docs.microsoft.com/en-us/graph/sdks/create-client?tabs=CS
            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(Configuration["AZURE_CLIENT_ID"])
                .WithTenantId(Configuration["AZURE_TENANT_ID"])
                .WithClientSecret(Configuration["AZURE_CLIENT_SECRET"])
                .Build();

            // Create an authentication provider
            // https://docs.microsoft.com/en-us/graph/sdks/choose-authentication-providers?tabs=CS#ClientCredentialsProvider
            ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);

            // Create a new instance of GraphServiceClient with the authentication provider.
            GraphServiceClient graphClient = new GraphServiceClient(authProvider);

            // If running locally, add a USER_EMAIL config to query against a user in AAD
            // If deployed in App Services, the Request Header can query against the signed in user principal
            // var user = await graphClient.Users[Configuration["USER_EMAIL"]].Request().GetAsync();
            var user = await graphClient.Users[Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"]].Request().GetAsync();

            return new JsonResult(user);
        }
    }
}