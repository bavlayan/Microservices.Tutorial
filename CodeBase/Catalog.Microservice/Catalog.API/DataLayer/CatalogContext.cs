using Catalog.API.DataLayer.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.DataLayer
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }

        public CatalogContext(ICatalogDatabaseSettings _catalogDatabaseSettings)
        {
            MongoClient mongoClient = new MongoClient(_catalogDatabaseSettings.ConnectionString);

            IMongoDatabase database = mongoClient.GetDatabase(_catalogDatabaseSettings.DatabaseName);

            Products = database.GetCollection<Product>(_catalogDatabaseSettings.CollectionName);

            CatalogContextSeed.SeedData(Products);
        }
    }
}
