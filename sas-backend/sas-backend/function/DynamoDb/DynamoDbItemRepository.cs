using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using function.model;

namespace function.DynamoDb
{
    public class DynamoDbItemRepository : IItemRepository
    {
        private readonly IDynamoDBContext _context;
        private readonly SasDynamoDbConfig _config;

        public DynamoDbItemRepository(IDynamoDBContext context, SasDynamoDbConfig config)
        {
            _context = context;
            _config = config;
        }

        public Task<IEnumerable<Item>> GetAllItems()
        {
            return _context.FromScanAsync<Item>(new ScanOperationConfig(), Cfg())
                .GetRemainingAsync()
                .ContinueWith(x =>
                {
                    return (IEnumerable<Item>) x.Result.OrderBy(y => y.Id);
                });
            //yes scan and return all items
        }

        public Task<Item> GetItemById(string id)
        {
            return _context.LoadAsync<Item>(id, Cfg());
        }

        private DynamoDBOperationConfig Cfg()
        {
            return new DynamoDBOperationConfig {OverrideTableName = _config.ItemsTableName};
        }
    }
}