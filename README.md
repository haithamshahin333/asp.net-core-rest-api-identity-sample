# App Patterns for Identity and Authentication/Authorization with App Services

This repo provides different patterns and examples for how to add authentication and authorization to connect to different APIs and Services in Azure.

## Deploy an App with App Services Easy Auth

[App Services Easy Auth](https://docs.microsoft.com/en-us/azure/app-service/overview-authentication-authorization#:~:text=Azure%20App%20Service%20provides%20built-in%20authentication%20and%20authorization,and%20mobile%20back%20end%2C%20and%20also%20Azure%20Functions.) provides a simple way to manage user authentication / authorization without modifying your application.

Here is a [tutorial](https://docs.microsoft.com/en-us/azure/app-service/configure-authentication-provider-aad) that walks through how to setup App Services Easy Auth. By running through these steps with this application, you will be able to:

1) Deploy the app to app services
2) Setup user authentication
3) Make further API calls using delegated permissions once setup with App Registration in Azure

You can view the swagger endpoint (`/swagger`) when the app is deployed and test the different endpoints available. For example, the `/headers` endpoint will output all the headers passed to your app, allowing you to view the different headers available once Easy Auth is setup.

## Deploy an App with App Services and Make Graph API Calls

The Graph Controller included in this app includes an example of making Graph API calls using the user information passed by Easy Auth in App Services.

The [Microsoft Graph SDKs](https://docs.microsoft.com/en-us/graph/sdks/sdk-installation) are leveraged for this example. Specifically, the Beta endpoint as well as the Microsoft Graph SDK.

Follow this [tutorial](https://docs.microsoft.com/en-us/azure/app-service/scenario-secure-app-access-microsoft-graph-as-app?tabs=azure-powershell%2Ccommand-line) to setup access to Microsoft Graph as a secured app (not delegated permissions in this example).

Once deployed, you can use the `/graphclient` endpoint to make a call to the Graph API. A request will be made against the signed-in user using the request header `X-MS-CLIENT-PRINCIPAL-NAME` passed by App Services Easy Auth.

## Authenticate to SQL Database (Azure SQL Database / Azure SQL Managed Instance) with User Access Token

[AAD Authentication with a SQL Database](https://docs.microsoft.com/en-us/azure/azure-sql/database/authentication-aad-overview) allows you to leverage your AAD Identities for centralized management.

Once a DB is provisioned and integrated with AAD for authentication, you can then deploy an App with delegated permissions to authenticate as the signed-in user to the SQL Instance through App Services Easy Auth.

This [reference](https://techcommunity.microsoft.com/t5/azure-database-support-blog/azure-sql-database-token-based-authentication-with-powershell/ba-p/369078) provides an example flow of how this works for a PowerShell app. The steps followed for this application are the same, except that App Services Easy Auth will do the work of obtaining and managing the Access Token which we can access and use through a Request Header.

### Steps

1) Deploy either Azure SQL or Azure SQL Managed Instance and [configure AAD Authentication](https://docs.microsoft.com/en-us/azure/azure-sql/database/authentication-aad-configure?tabs=azure-powershell)
2) Deploy the App to App Services and Setup Easy Auth
3) Register the "Azure SQL Database" app by running `New-AzureADServicePrincipal -AppId “022907d3-0f1b-48f7-badc-1ba6abab6d66”  -DisplayName “Azure SQL Database”`
4) Add a delegated App Permission for the App Registration that was made during the Easy Auth setup
   a) Navigate to your App Registration
   b) Select API Permissions
   c) Add a permission
   d) Search for Azure SQL Database under 'APIs my organization uses' (if you don't see anything appear, review step 3 above)
   e) Selected Delegated Permissions and add user_impersonation
5) Navigate to your App Service Web App and select Configuration in order to add the DB connectionString
   a) Add a new connection string with key: connectionString, value: `Server=<ENDPOINT OF DB>; Database=<DATABASE NAME>`
   b) Make the type 'SQLServer'
6) The app should restart after configuring the connectionString
7) Navigate to the App Url and on the `/swagger` page, select the `/sqlconnection` endpoint which runs a basic query against the DB.

> Info: Make sure the user that you sign into the App with is configured for access to the SQL Instance with AAD.

# Resources
- https://docs.microsoft.com/en-us/dotnet/api/overview/azure/app-auth-migration
- https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-permissions-and-consent#the-default-scope
- https://docs.microsoft.com/en-us/azure/app-service/scenario-secure-app-access-microsoft-graph-as-app?tabs=azure-powershell%2Ccommand-line