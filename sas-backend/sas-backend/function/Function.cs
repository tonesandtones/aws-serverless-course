using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using function.model;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace sas_backend
{
    public class Functions
    {
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions()
        {
        }

        public static ILambdaContext Context;

        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of blogs</returns>
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            Context = context;
            context.Logger.LogLine("Get Request\n");

            object responseBody = null;
            var testData = new TestDataAccessor().TestData;

            var itemId = request?.PathParameters?["id"];
            if (itemId == null)
            {
                responseBody = new {Items = testData.Items};
            } else
            {
                responseBody = testData.Items.FirstOrDefault(x => x.Id.Equals(itemId, StringComparison.InvariantCultureIgnoreCase));
            }
            
            context.Logger.LogLine("got response body");
            
            var response = new APIGatewayProxyResponse
            {
                StatusCode = responseBody == null ? (int)HttpStatusCode.NotFound : (int) HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(responseBody),
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"},
                    {"Access-Control-Allow-Origin", "*"}
                }
            };
            
            context.Logger.LogLine("prepared response entity");

            return response;
        }
    }

    public class TestDataAccessor
    {
        private TestData _testData;

        public TestData TestData
        {
            get
            {
                if (_testData == null)
                {
                    _testData = ReadTestData();
                }

                return _testData;
            }
        }

        private TestData ReadTestData()
        {
            var raw = File.ReadAllText(@"resources/testdata.json");
            Functions.Context.Logger.LogLine($"got test data text:\n{raw}");
            return JsonConvert.DeserializeObject<TestData>(raw);
        }
    }
}