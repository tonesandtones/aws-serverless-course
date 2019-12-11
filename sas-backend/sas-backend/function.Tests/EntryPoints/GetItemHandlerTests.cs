using System;
using System.Threading.Tasks;
using Amazon.Lambda.TestUtilities;
using AutoBogus;
using DeepEqual.Syntax;
using function.EntryPoints;
using function.model;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace function.Tests.EntryPoints
{
    public class GetItemHandlerTests
    {
        [Fact]
        public async Task GetItemReShouldReturn404WhenNoItemAvailable()
        {
            var testItems = AutoFaker.Generate<Item>(50);
            var resolver = TestSasServices.DefaultServiceCollection()
                .WithDefaultFunctionHandlers()
                .WithTestItems(testItems)
                .BuildServiceProvider();

            var handler = resolver.GetRequiredService<GetItemByIdHandler>();
            var request = new ApiGatewayProxyRequestBuilder()
                .WithPathParameter("id", Guid.NewGuid().ToString())
                .Build();

            var response = await handler.HandleAsync(request, new TestLambdaContext());
            response
                .ShouldHaveStatusCode(404)
                .Body
                .ShouldBeNull();
        }

        [Fact]
        public async Task GetItemReShouldReturn200WithFoundItem()
        {
            var items = new AutoFaker<Item>()
                .RuleFor(x => x.Id, x => x.UniqueIndex.ToString()) //can sometimes get duplicates, so force unique indexes
                .Generate(50);
            var item = items[new Random().Next(0, items.Count)];

            var resolver = TestSasServices.DefaultServiceCollection()
                .WithDefaultFunctionHandlers()
                .WithTestItems(items)
                .BuildServiceProvider();

            var handler = resolver.GetRequiredService<GetItemByIdHandler>();
            var request = new ApiGatewayProxyRequestBuilder().WithPathParameter("id", item.Id).Build();
            var response = await handler.HandleAsync(request, new TestLambdaContext());
            response
                .ShouldHaveStatusCode(200)
                .ShouldHaveHeader("Content-Type", "application/json")
                .ShouldHaveHeader("Access-Control-Allow-Origin", "*")
                .Body
                .ShouldBeParseableAs<Item>()
                .ShouldDeepEqual(item);
        }
    }
}