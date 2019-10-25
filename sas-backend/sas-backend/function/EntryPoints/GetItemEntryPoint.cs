using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
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
                .AddTransient<IHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>, GetItemHandler>();
    }

    public class GetItemHandler : IHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>
    {
        private readonly IItemRepository _items;
        private readonly IResponseBuilderFactory _builder;

        public GetItemHandler(IItemRepository items, IResponseBuilderFactory builder)
        {
            _items = items;
            _builder = builder;
        }

        public async Task<APIGatewayProxyResponse> HandleAsync(APIGatewayProxyRequest input, ILambdaContext context)
        {
            var itemId = input?.PathParameters?["id"];
            var responseBody = _items.GetItemById(itemId);

            return _builder.Create()
                .WithDefaultsForEntity(responseBody)
                .Build();
        }
    }
}