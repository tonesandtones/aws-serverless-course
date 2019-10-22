using System.IO;
using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Hosting;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace sas_backend
{
    public class LambdaEntryPoint : APIGatewayProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder.UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseLambdaServer();
        }
    }
}