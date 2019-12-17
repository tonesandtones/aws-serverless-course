using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using function.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tiger.Lambda;

namespace function.EntryPoints
{
    public class GetItemEntryPoint : Function<APIGatewayProxyRequest, APIGatewayProxyResponse>
    {
        public override void ConfigureServices(HostBuilderContext context, IServiceCollection services) =>
            services
                .AddSasServices()
                .AddHandler<GetItemByIdHandler, APIGatewayProxyRequest, APIGatewayProxyResponse>();
    }

    public class GetItemByIdHandler : IHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>
    {
        private readonly IItemRepository _items;
        private readonly IFactory<IBuilder<APIGatewayProxyResponse>> _response;

        public GetItemByIdHandler(IItemRepository items, IFactory<IBuilder<APIGatewayProxyResponse>> response)
        {
            _items = items;
            _response = response;
        }

        public async Task<APIGatewayProxyResponse> HandleAsync(APIGatewayProxyRequest input, ILambdaContext context)
        {
            var itemId = input?.PathParameters?["id"];
            if (string.IsNullOrEmpty(itemId))
            {
                return _response.Create()
                    .WithDefaultErrorEntity(400, "No id specified")
                    .Build();
            }

            var responseBody = await _items.GetItemById(itemId);

            return _response.Create()
                .WithDefaultsForEntity(responseBody)
                .Build();
        }
    }
}