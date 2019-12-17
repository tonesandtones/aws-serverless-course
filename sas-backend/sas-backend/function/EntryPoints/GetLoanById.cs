using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using function.Builders;
using function.Repositories;
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
                .AddHandler<GetLoanByIdHandler, APIGatewayProxyRequest, APIGatewayProxyResponse>();
    }

    public class GetLoanByIdHandler : IHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>
    {
        private readonly ILoanRepository _loans;
        private readonly IFactory<IBuilder<APIGatewayProxyResponse>> _response;

        public GetLoanByIdHandler(ILoanRepository loans, IFactory<IBuilder<APIGatewayProxyResponse>> response)
        {
            _loans = loans;
            _response = response;
        }

        public async Task<APIGatewayProxyResponse> HandleAsync(APIGatewayProxyRequest input, ILambdaContext context)
        {
            var id = input.PathParameter("id");
            if (string.IsNullOrEmpty(id))
            {
                return _response.Create()
                    .WithDefaultErrorEntity(400, "No id specified")
                    .Build();
            }

            var responseBody = await _loans.GetLoanById(id);
            return _response.Create()
                .WithDefaultsForEntity(responseBody)
                .Build();
        }
    }
}