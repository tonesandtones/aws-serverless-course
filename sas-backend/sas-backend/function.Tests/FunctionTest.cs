using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace function.Tests
{
    public class FunctionTest
    {
        public FunctionTest()
        {
        }

        [Fact]
        public async Task TetGetMethod()
        {
            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

//            var functions = new LambdaEntryPoint();
//
//            request = new APIGatewayProxyRequest
//            {
//                Path = "/items"
//            };
//            context = new TestLambdaContext();
//            response = await functions.FunctionHandlerAsync(request, context);
//            response.StatusCode.ShouldBe(200);
//            JsonConvert.DeserializeObject(response.Body).ShouldNotBeNull();
        }
    }
}
