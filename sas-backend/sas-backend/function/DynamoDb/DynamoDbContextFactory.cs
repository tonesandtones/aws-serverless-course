using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace function.DynamoDb
{
    public class DynamoDbContextFactory : IDynamoDbContextFactory
    {
        private readonly AmazonDynamoDBClient _client;

        public DynamoDbContextFactory(AmazonDynamoDBClient client)
        {
            _client = client;
        }

        public DynamoDBContext Create()
        {
            return new DynamoDBContext(_client);
        }
    }
}