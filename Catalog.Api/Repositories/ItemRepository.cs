using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Api.Interfaces;
using Catalog.Api.Models;

namespace Catalog.Api.Repositories
{
    /*  public class ItemRepository : IItemRepository
      {
          private List<Item> _items = new()
          {
              new Item { Id = Guid.NewGuid(), Name = "HP", Description = "Laptop", Price = 200000, CreationTime = DateTimeOffset.UtcNow },
              new Item { Id = Guid.NewGuid(), Name = "Samsung", Description = "Smart Phone", Price = 30000, CreationTime = DateTimeOffset.UtcNow }

          };
          public void CreateItemAsync(Item item)
          {
              _items.Add(item);
          }

          public void DeleteItem(Item item)
          {
              _items.Remove(item);
          }

          public void DeleteItemAsync(Guid id)
          {
              throw new NotImplementedException();
          }

          public Item GetItemAsync(Guid id)
          {
              return _items.Where(item => item.Id == id).SingleOrDefault();
          }

          public IEnumerable<Item> GetItemsAsync()
          {
              return _items;
          }

          public void Async(Item item)
          {
              var index = _items.FindIndex(element => element.Id == item.Id);
              _items[index] = item;
          }
      }*/

}