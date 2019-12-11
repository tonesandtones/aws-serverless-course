using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using function.Builders;
using function.model;
using function.Repositories;
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
        private readonly IFactory<IBuilder<APIGatewayProxyResponse>> _response;

        public GetLoansByStatusHandler(ILoanRepository loans, IFactory<IBuilder<APIGatewayProxyResponse>> response)
        {
            _loans = loans;
            _response = response;
        }

        public async Task<APIGatewayProxyResponse> HandleAsync(APIGatewayProxyRequest input, ILambdaContext context)
        {
            var statusStr = input.PathParameter("status");
            
            bool canParse = Enum.TryParse(statusStr, true, out LoanStatus status);
            if (!canParse)
            {
                return _response.Create()
                    .WithDefaultErrorEntity(HttpStatusCode.BadRequest,
                        $"Could not parse path parameter 'status' with value {statusStr}. " +
                        $"Requires one of [{string.Join(", ", Enum.GetNames(typeof(LoanStatus)))}]"
                    )
                    .Build();
            }

            var responseBody = _loans.GetLoansByStatus(status);
            return _response.Create()
                .WithDefaultsForEntity(responseBody)
                .Build();
        }
    }
}