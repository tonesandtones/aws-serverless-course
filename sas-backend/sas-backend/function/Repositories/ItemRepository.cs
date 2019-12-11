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
        private readonly ITestDataAccessor _accessor;

        public ItemRepository(ITestDataAccessor accessor)
        {
            _accessor = accessor;
        }

        public IEnumerable<Item> GetAllItems()
        {
            return new List<Item>(_accessor.Items);
        }

        public Item GetItemById(string id)
        {
            if (id == null)
            {
                return null;
            }

            return _accessor.Items.FirstOrDefault(x => string.Equals(x.Id,id, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}