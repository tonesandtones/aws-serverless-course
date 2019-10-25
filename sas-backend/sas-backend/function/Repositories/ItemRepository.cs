using System;
using System.Collections.Generic;
using System.Linq;
using function.model;

namespace function
{
    public interface IItemRepository
    {
        IEnumerable<Item> GetAllItems();
        Item GetItemById(string id);
    }

    public class ItemRepository : IItemRepository
    {
        private readonly TestDataAccessor _accessor;

        public ItemRepository(TestDataAccessor accessor)
        {
            _accessor = accessor;
        }

        public IEnumerable<Item> GetAllItems()
        {
            return new List<Item>(_accessor.TestData.Items);
        }

        public Item GetItemById(string id)
        {
            if (id == null)
            {
                return null;
            }

            return _accessor.TestData.Items.FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}