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
    public class GetLoanById : Function<APIGatewayProxyRequest, APIGatewayProxyResponse>
    {
        public override void ConfigureServices(HostBuilderContext context, IServiceCollection services) =>
            services
                .AddSasServices()
                .AddTransient<IHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>, GetLoanByIdHandler>();

        public class GetLoanByIdHandler : IHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>
        {
            private readonly ILoanRepository _loans;
            private readonly IResponseBuilderFactory _builder;

            public GetLoanByIdHandler(ILoanRepository loans, IResponseBuilderFactory builder)
            {
                _loans = loans;
                _builder = builder;
            }

            public async Task<APIGatewayProxyResponse> HandleAsync(APIGatewayProxyRequest input, ILambdaContext context)
            {
                var id = input.PathParameter("id");
                var responseBody = _loans.GetLoanById(id);
                return _builder.Create()
                    .WithDefaultsForEntity(responseBody)
                    .Build();
            }
        }
    }
}