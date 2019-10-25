using System.Collections.Generic;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

namespace function
{
    public interface IResponseConverter
    {
        APIGatewayProxyResponse Convert(object responseBody);
    }

    public class ApiGatewayProxyResponseConverter : IResponseConverter
    {
        public APIGatewayProxyResponse Convert(object responseBody)
        {
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
            return response;
        }
    }
}