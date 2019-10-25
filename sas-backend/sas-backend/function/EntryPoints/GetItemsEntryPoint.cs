using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
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

        public class GetItemsHandler : IHandler<APIGatewayProxyRequest, APIGatewayProxyResponse>
        {
            private readonly IItemRepository _items;
            private readonly IResponseBuilderFactory _builder;

            public GetItemsHandler(IItemRepository items, IResponseBuilderFactory builder)
            {
                _items = items;
                _builder = builder;
            }

            public async Task<APIGatewayProxyResponse> HandleAsync(APIGatewayProxyRequest input, ILambdaContext context)
            {
                var responseBody = _items.GetAllItems();
                return _builder.Create()
                    .WithDefaultsForEntity(responseBody)
                    .Build();
            }
        }
    }
}