using System.Collections.Generic;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

namespace function
{
    public interface IResponseBuilderFactory
    {
        IResponseBuilder Create();
    }

    public class ApiGatewayProxyResponseBuilderFactory : IResponseBuilderFactory
    {
        public IResponseBuilder Create()
        {
            return new ApiGatewayProxyResponseBuilder();
        }
    }

    public interface IResponseBuilder
    {
        APIGatewayProxyResponse Build();

        IResponseBuilder WithStatusCode(int statusCode);
        IResponseBuilder WithStatusCode(HttpStatusCode statusCode);
        IResponseBuilder WithResponseEntity(object responseEntity);
        IResponseBuilder WithDefaultCorsHeaders();
        IResponseBuilder WithStatusCodeDerivedFromEntity();
        IResponseBuilder WithDefaultsForEntity(object entity);
    }

    public class ApiGatewayProxyResponseBuilder : IResponseBuilder
    {
        private int _statusCode = 400;
        private bool _hasBody = false;
        private object _responseEntity = null;
        private readonly IDictionary<string, string> _headers = new Dictionary<string, string>();
        private bool _deriveStatusCodeFromEntity = false;

        public APIGatewayProxyResponse Build()
        {
            var statusCodeBasedOnEntityPresence = _responseEntity == null ? 404 : 200;
            var statusCode = _deriveStatusCodeFromEntity ? statusCodeBasedOnEntityPresence : _statusCode;
            var response = new APIGatewayProxyResponse
            {
                StatusCode = statusCode,
                Body = _hasBody ? JsonConvert.SerializeObject(_responseEntity) : null,
                Headers = _headers
            };
            return response;
        }

        public IResponseBuilder WithStatusCode(int statusCode)
        {
            _statusCode = statusCode;
            return this;
        }

        public IResponseBuilder WithStatusCode(HttpStatusCode statusCode)
        {
            return WithStatusCode((int) statusCode);
        }

        public IResponseBuilder WithResponseEntity(object responseEntity)
        {
            _hasBody = true;
            _responseEntity = responseEntity;
            return this;
        }

        public IResponseBuilder WithDefaultCorsHeaders()
        {
            _headers.Add("Content-Type", "application/json");
            _headers.Add("Access-Control-Allow-Origin", "*");
            return this;
        }

        public IResponseBuilder WithStatusCodeDerivedFromEntity()
        {
            _deriveStatusCodeFromEntity = true;
            return this;
        }

        public IResponseBuilder WithDefaultsForEntity(object entity)
        {
            return WithStatusCodeDerivedFromEntity()
                .WithResponseEntity(entity)
                .WithDefaultCorsHeaders();
        }
    }
}