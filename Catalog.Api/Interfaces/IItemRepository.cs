using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api.Models;

namespace Catalog.Api.Interfaces
{
    public interface IItemRepository
    {
        Task CreateItemAsync(Item item);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task<Item> GetItemAsync(Guid id);
        Task DeleteItemAsync(Guid id);
        Task UpdateItemAsync(Item item);

    }
}