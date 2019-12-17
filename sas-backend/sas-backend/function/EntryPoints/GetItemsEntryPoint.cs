using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using function.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tiger.Lambda;

namespace function.EntryPoints
{
    public class GetItemsEntryPoint : Function<APIGatewayProxyRequest, APIGatewayProxyResponse>
    {
        public override void ConfigureServices(HostBuilderContext context, IServiceCollection services) =>
            services
                .AddSasServices()
                .AddTransient<IHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>, GetItemsHandler>();
    }

    public class GetItemsHandler : IHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>
    {
        private readonly IItemRepository _items;
        private readonly IFactory<IBuilder<APIGatewayProxyResponse>> _response;

        public GetItemsHandler(IItemRepository items, IFactory<IBuilder<APIGatewayProxyResponse>> response)
        {
            _items = items;
            _response = response;
        }

        public async Task<APIGatewayProxyResponse> HandleAsync(APIGatewayProxyRequest input, ILambdaContext context)
        {
            var responseBody = await _items.GetAllItems();
            return _response.Create()
                .WithDefaultsForEntity(responseBody)
                .Build();
        }
    }
}