using System.Collections.Generic;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using function.model;
using Newtonsoft.Json;

namespace function.Builders
{
    public class ApiGatewayProxyResponseBuilder : AbstractBuilder<APIGatewayProxyResponse>
    {
        protected override APIGatewayProxyResponse InitialiseEmpty()
        {
            return new APIGatewayProxyResponse()
            {
                StatusCode = 400, //default status code if nothing changes it.
                Headers = new Dictionary<string, string>()
            };
        }
    }

    public static class ApiGatewayProxyResponseBuilderExtensions
    {
        public static T WithStatusCode<T>(this T builder, int statusCode)
            where T : IBuilder<APIGatewayProxyResponse>
        {
            builder.AppendAction(x => x.StatusCode = statusCode);
            return builder;
        }

        public static T WithStatusCode<T>(this T builder, HttpStatusCode statusCode)
            where T : IBuilder<APIGatewayProxyResponse>
        {
            return builder.WithStatusCode((int) statusCode);
        }

        public static T WithResponseEntity<T>(this T builder, object entity)
            where T : IBuilder<APIGatewayProxyResponse>
        {
            builder.AppendAction(x => x.Body = (entity == null ? null : JsonConvert.SerializeObject(entity)));
            return builder;
        }

        public static T WithDefaultsForEntity<T>(this T builder, object entity)
            where T : IBuilder<APIGatewayProxyResponse>
        {
            return builder.WithResponseEntity(entity)
                .WithDefaultCorsHeaders()
                .WithStatusCodeDerivedFromEntity();
        }

        public static T WithStatusCodeDerivedFromEntity<T>(this T builder)
            where T : IBuilder<APIGatewayProxyResponse>
        {
            builder.AppendAction(x => x.StatusCode = (string.IsNullOrEmpty(x.Body) ? 404 : 200));
            return builder;
        }

        public static T WithDefaultCorsHeaders<T>(this T builder)
            where T : IBuilder<APIGatewayProxyResponse>
        {
            builder.AppendAction(x =>
            {
                x.Headers.Add("Content-Type", "application/json");
                x.Headers.Add("Access-Control-Allow-Origin", "*");
            });
            return builder;
        }

        public static T WithDefaultErrorEntity<T>(this T builder, int statusCode, string message = null)
            where T : IBuilder<APIGatewayProxyResponse>
        {
            builder
                .WithDefaultsForEntity(new ErrorResponse() {StatusCode = statusCode, Error = message})
                .WithStatusCode(statusCode); //replace the status code with the one we want.
            return builder;
        }
        
        public static T WithDefaultErrorEntity<T>(this T builder, HttpStatusCode statusCode, string message = null)
            where T : IBuilder<APIGatewayProxyResponse>
        {
            builder
                .WithDefaultsForEntity(new ErrorResponse() {StatusCode = (int)statusCode, Error = message})
                .WithStatusCode(statusCode); //replace the status code with the one we want.
            return builder;
        }
    }
}