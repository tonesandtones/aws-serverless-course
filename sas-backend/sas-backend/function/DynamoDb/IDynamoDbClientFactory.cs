using Amazon;
using Amazon.DynamoDBv2;

namespace function.DynamoDb
{
    public interface IDynamoDbClientFactory
    {
        AmazonDynamoDBClient Create();
    }

    public class DynamoDbClientFactory : IDynamoDbClientFactory
    {
        private readonly SasDynamoDbConfig _config;

        public DynamoDbClientFactory(SasDynamoDbConfig config)
        {
            _config = config;
        }

        public AmazonDynamoDBClient Create()
        {
            var dynamoConfig = new AmazonDynamoDBConfig();
            if (!string.IsNullOrEmpty(_config.EndpointUrl))
            {
                dynamoConfig.ServiceURL = _config.EndpointUrl;
            }
            // if (!string.IsNullOrEmpty(_config.RegionEndpoint))
            // {
            // dynamoConfig.RegionEndpoint = RegionEndpoint.GetBySystemName(_config.RegionEndpoint);
            // }

            return new AmazonDynamoDBClient(dynamoConfig);
        }
    }
}