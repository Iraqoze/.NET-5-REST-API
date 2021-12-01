using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Interfaces;
using Catalog.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.UnitTests
{
    public class ItemControllerTests
    {
        private readonly Mock<IItemRepository> repositoryStub = new();
        private readonly Mock<ILogger<ItemController>> loggerStub = new();
        private readonly Random rand = new();

        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            //arrange            
            repositoryStub.Setup(repos => repos.GetItemAsync(It.IsAny<Guid>()))
                        .ReturnsAsync((Item)null);

            var controller = new ItemController(repositoryStub.Object, loggerStub.Object);

            //act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            //assert
            result.Result.Should().BeOfType<NotFoundResult>();

        }
        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
        {
            //arrange
            var expectedItem = CreateRandomItem();
            repositoryStub.Setup(repos => repos.GetItemAsync(It.IsAny<Guid>()))
                        .ReturnsAsync(expectedItem);
            var controller = new ItemController(repositoryStub.Object, loggerStub.Object);

            //act
            var result = await controller.GetItemAsync(Guid.NewGuid());

            //Assert
            result.Value.Should().BeEquivalentTo(expectedItem, options => options.ComparingByMembers<Item>());

        }

        [Fact]
        public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
        {
            //arrange
            var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };
            repositoryStub.Setup(repos => repos.GetItemsAsync()).ReturnsAsync(expectedItems);
            var controller = new ItemController(repositoryStub.Object, loggerStub.Object);

            //act
            var actualIems = await controller.GetItemsAsync();

            //assert
            actualIems.Should().BeEquivalentTo(expectedItems, options => options.ComparingByMembers<Item>());

        }
        [Fact]
        public async Task GetItemsAsync_WithMatchingItems_ReturnsMatchingItems()
        {
            //arrange
            var allItems = new[] {
               new Item{Name="Car"},
               new Item{Name="Meals on Wheels"},
               new Item{Name="Toyota Car"},
               new Item{Name="LeeCar"}

            };
            var nameToMatch = "Car";
            repositoryStub.Setup(repos => repos.GetItemsAsync()).ReturnsAsync(allItems);
            var controller = new ItemController(repositoryStub.Object, loggerStub.Object);

            //act
            IEnumerable<ItemDto> foundItems = await controller.GetItemsAsync(nameToMatch);

            //assert
            foundItems.Should().OnlyContain(
                item => item.Name == allItems[0].Name || item.Name == allItems[3].Name
                || item.Name == allItems[2].Name
            );

        }
        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {

            //arrange
            var itemToCreate = new CreateItemDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), rand.Next(200000));

            var controller = new ItemController(repositoryStub.Object, loggerStub.Object);

            //act
            var result = await controller.CreateItemAsync(itemToCreate);

            //assert
            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
            createdItem.Should().BeEquivalentTo(itemToCreate, options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers());

            createdItem.Id.Should().NotBeEmpty();
            createdItem.Description.Should().NotBeEmpty();
            //createdItem.CreationTime.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        }
        [Fact]
        public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
        {
            //arrange
            Item existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(existingItem);

            var itemToUpdate = new UpdateItemDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), existingItem.Price + 50);

            var controller = new ItemController(repositoryStub.Object, loggerStub.Object);

            //act
            var result = await controller.UpdateItemAsync(existingItem.Id, itemToUpdate);

            //assert
            result.Should().BeOfType<NoContentResult>();

        }
        [Fact]
        public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
        {
            //arrange
            Item existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                            .ReturnsAsync(existingItem);


            var controller = new ItemController(repositoryStub.Object, loggerStub.Object);

            //act
            var result = await controller.DeleteItemAsync(existingItem.Id);

            //assert
            result.Should().BeOfType<NoContentResult>();

        }

        private Item CreateRandomItem()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Price = rand.Next(200000),
                CreationTime = DateTimeOffset.UtcNow

            };
        }
    }
}
