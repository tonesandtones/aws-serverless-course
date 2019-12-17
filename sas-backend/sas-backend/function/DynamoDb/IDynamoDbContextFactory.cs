using Amazon.DynamoDBv2.DataModel;

namespace function.DynamoDb
{
    public interface IDynamoDbContextFactory
    {
        DynamoDBContext Create();
    }
}