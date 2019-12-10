using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;

namespace function.Tests.EntryPoints
{
    public static class ApiGatewayProxyRequestBuilderExtensions
    {
        public static T WithPathParameter<T>(this T builder, string pathParam, string value)
            where T : IBuilder<APIGatewayProxyRequest>
        {
            builder.WithAction(x => x.PathParameters[pathParam] = value);
            return builder;
        }

        public static T WithQueryParameter<T>(this T builder, string queryParam, string value)
            where T : IBuilder<APIGatewayProxyRequest>
        {
            builder.WithAction(x => x.QueryStringParameters[queryParam] = value);
            return builder;
        }
    }

    public class ApiGatewayProxyRequestBuilder : AbstractBuilder<APIGatewayProxyRequest>
    {
        protected override APIGatewayProxyRequest InitialiseEmpty()
        {
            var request = new APIGatewayProxyRequest
            {
                PathParameters = new Dictionary<string, string>(),
                Headers = new Dictionary<string, string>(),
                MultiValueHeaders = new Dictionary<string, IList<string>>(),
                MultiValueQueryStringParameters = new Dictionary<string, IList<string>>(),
                QueryStringParameters = new Dictionary<string, string>(),
                StageVariables = new Dictionary<string, string>()
            };

            return request;
        }
    }
}