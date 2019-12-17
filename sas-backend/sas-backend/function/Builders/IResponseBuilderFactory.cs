using Amazon.Lambda.APIGatewayEvents;

namespace function.Builders
{
    public class ApiGatewayProxyResponseBuilderFactory : IFactory<IBuilder<APIGatewayProxyResponse>>
    {
        public IBuilder<APIGatewayProxyResponse> Create()
        {
            return new ApiGatewayProxyResponseBuilder();
        }
    }

    public interface IFactory<out T>
    {
        T Create();
    }
}