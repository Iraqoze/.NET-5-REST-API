using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api.Interfaces;
using Catalog.Api.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    //Mongo DB Docker Image
    //docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo
    //docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word --network=Net5RestAPITutorial mongo
    //docker run -it --rm -p 8080:80 -e MongoDbSettings:Host=mongo -e MongoDbSettings:Password=Pass#word --network=Net5RestAPITutorial lirakoze/catalog:v1
    public class MongoDbItemRepository : IItemRepository
    {
        private const string DatabaseName = "Catalog";
        private const string CollectionName = "Items";
        private readonly IMongoCollection<Item> _itemsCollection;

        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public MongoDbItemRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(DatabaseName);
            _itemsCollection = database.GetCollection<Item>(CollectionName);
        }
        public async Task CreateItemAsync(Item item)
        {
            await _itemsCollection.InsertOneAsync(item);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            await _itemsCollection.DeleteOneAsync(filter);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            return await _itemsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await _itemsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await _itemsCollection.ReplaceOneAsync(filter, item);
        }
    }

}