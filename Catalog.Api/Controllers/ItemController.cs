using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Dtos;
using Catalog.Api.Extensions;
using Catalog.Api.Interfaces;
using Catalog.Api.Models;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.Api.Controllers
{
    //Get Items
    [ApiController]
    [Route("items")]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _repository;
        private readonly ILogger<ItemController> _logger;
        public ItemController(IItemRepository repos, ILogger<ItemController> logger)
        {
            _repository = repos;
            _logger = logger;
        }

        //Get Items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync(string nameToMatch = null)
        {

            var items = (await _repository.GetItemsAsync()).Select(item => item.AsDto());

            if (!string.IsNullOrWhiteSpace(nameToMatch))
            {
                items = items.Where(item => item.Name.Contains(nameToMatch, StringComparison.OrdinalIgnoreCase));
            }
            _logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: The Number of Products is: {items.Count()}");
            return items;
        }

        //Get Specific Item
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await _repository.GetItemAsync(id);
            if (item is null)
                return NotFound();
            return item.AsDto();
        }


        //Add Item
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price,
                CreationTime = DateTimeOffset.UtcNow
            };
            await _repository.CreateItemAsync(item);

            _logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: New Item is added: {item.Name}");

            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
        }

        //Delete Item
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var item = _repository.GetItemAsync(id);
            if (item is null)
                NotFound();
            await _repository.DeleteItemAsync(id);
            _logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Item has been deleted");
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await _repository.GetItemAsync(id);

            if (existingItem is null)
                return NotFound();

            existingItem.Name = itemDto.Name;
            existingItem.Description = itemDto.Description;
            existingItem.Price = itemDto.Price;

            await _repository.UpdateItemAsync(existingItem);
            _logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Item {itemDto.Name} has been updated");
            return NoContent();


        }


    }

}