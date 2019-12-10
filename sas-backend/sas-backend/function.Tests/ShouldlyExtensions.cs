using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Shouldly;

namespace function.Tests.EntryPoints
{
    public static class ShouldlyExtensions
    {
        public static APIGatewayProxyResponse ShouldHaveStatusCode(this APIGatewayProxyResponse response, int statusCode)
        {
            response.StatusCode.ShouldBe(statusCode);
            return response;
        }

        public static T ShouldBeParseableAs<T>(this string body)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<T>(body);
                return result;
            }
            catch (JsonException e)
            {
                throw new ShouldAssertException($"Should have been able to parse input as a {typeof(T).Name} but could not.", e);
            }
        }

        public static APIGatewayProxyResponse ShouldHaveHeader(this APIGatewayProxyResponse response, string header, string value)
        {
            response.Headers.ShouldContainKeyAndValue(header, value);
            return response;
        }
    }
}