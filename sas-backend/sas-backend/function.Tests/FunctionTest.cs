using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using sas_backend;
using Shouldly;

namespace sas_backend.Tests
{
    public class FunctionTest
    {
        public FunctionTest()
        {
        }

        [Fact]
        public void TetGetMethod()
        {
            TestLambdaContext context;
            APIGatewayProxyRequest request;
            APIGatewayProxyResponse response;

            Functions functions = new Functions();

            request = new APIGatewayProxyRequest
            {
                Path = "/items"
            };
            context = new TestLambdaContext();
            response = functions.Get(request, context);
            response.StatusCode.ShouldBe(200);
            JsonConvert.DeserializeObject(response.Body).ShouldNotBeNull();
        }
    }
}
