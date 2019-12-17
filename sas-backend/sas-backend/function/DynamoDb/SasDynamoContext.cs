using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

namespace function.DynamoDb
{
    public class SasDynamoContext
    {
        private Table _loans;
        private Table _items;

        public Table Loans
        {
            get
            {
                if (_loans == null)
                {
                    _loans = Table.LoadTable(_client, _config.LoansTableName);
                }

                return _loans;
            }
        }

        public Table Items
        {
            get
            {
                if (_items == null)
                {
                    _items = Table.LoadTable(_client, _config.ItemsTableName);
                }

                return _items;
            }
        }

        private readonly SasDynamoDbConfig _config;
        private readonly AmazonDynamoDBClient _client;

        public SasDynamoContext(SasDynamoDbConfig config, AmazonDynamoDBClient client)
        {
            _config = config;
            _client = client;
        }
    }
}