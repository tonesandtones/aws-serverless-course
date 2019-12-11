using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.TestUtilities;
using AutoBogus;
using DeepEqual.Syntax;
using function.EntryPoints;
using function.model;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace function.Tests.EntryPoints
{
    public class GetItemsHandlerTests
    {
        [Theory]
        [MemberData(nameof(ItemsData))]
        public async Task TestGetItemsHandler(IEnumerable<Item> items)
        {
            var resolver = TestSasServices
                .DefaultServiceCollection()
                .WithDefaultFunctionHandlers()
                .WithTestItems(items)
                .BuildServiceProvider();

            var handler = resolver.GetRequiredService<GetItemsHandler>();

            var request = new ApiGatewayProxyRequestBuilder().Build();
            var lambdaContext = new TestLambdaContext();
            var response = await handler.HandleAsync(request, lambdaContext);

            response
                .ShouldHaveStatusCode(200)
                .ShouldHaveHeader("Content-Type", "application/json")
                .Body
                .ShouldBeParseableAs<IList<Item>>()
                .ShouldDeepEqual(items);
        }

        public static IEnumerable<object[]> ItemsData()
        {
            return new List<object[]>
            {
                new object[] {AutoFaker.Generate<Item>(50)},
                new object[] {new List<Item>()} //empty list
            };
        }
    }
}