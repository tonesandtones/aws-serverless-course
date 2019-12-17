using System.Collections.Generic;
using System.Threading.Tasks;
using function.model;

namespace function
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllItems();
        Task<Item> GetItemById(string id);
    }
}