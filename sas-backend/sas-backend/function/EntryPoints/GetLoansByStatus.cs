using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using function.model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tiger.Lambda;

namespace function.EntryPoints
{
    public class GetLoansByStatus : Function<APIGatewayProxyRequest, APIGatewayProxyResponse>
    {
        public override void ConfigureServices(HostBuilderContext context, IServiceCollection services) =>
            services
                .AddSasServices()
                .AddTransient<IHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>, GetLoansByStatusHandler>();
    }

    public class GetLoansByStatusHandler : IHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>
    {
        private readonly ILoanRepository _loans;
        private readonly IResponseBuilderFactory _builder;

        public GetLoansByStatusHandler(ILoanRepository loans, IResponseBuilderFactory builder)
        {
            _loans = loans;
            _builder = builder;
        }

        public async Task<APIGatewayProxyResponse> HandleAsync(APIGatewayProxyRequest input, ILambdaContext context)
        {
            var statusStr = input.PathParameter("status");
            bool canParse = Enum.TryParse(statusStr, true, out LoanStatus status);
            if (!canParse)
            {
                return _builder.Create()
                    .WithDefaultCorsHeaders()
                    .WithStatusCode(HttpStatusCode.BadRequest)
                    .WithResponseEntity(new
                    {
                        Error = $"Could not parse path parameter 'status' with value {statusStr}. " +
                                $"Requires one of [{String.Join(", ", Enum.GetNames(typeof(LoanStatus)))}]"
                    })
                    .Build();
            }

            var responseBody = _loans.GetLoansByStatus(status);
            return _builder.Create()
                .WithDefaultsForEntity(responseBody)
                .Build();
        }
    }
}