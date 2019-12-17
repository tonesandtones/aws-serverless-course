using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using function.model;

namespace function.Repositories
{
    public class TestDataItemRepository : IItemRepository
    {
        private readonly ITestDataAccessor _accessor;

        public TestDataItemRepository(ITestDataAccessor accessor)
        {
            _accessor = accessor;
        }

        public Task<IEnumerable<Item>> GetAllItems()
        {
            return Task.FromResult((IEnumerable<Item>) new List<Item>(_accessor.Items));
        }

        public Task<Item> GetItemById(string id)
        {
            if (id == null)
            {
                return Task.FromResult((Item) null);
            }

            return Task.FromResult(_accessor.Items.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}