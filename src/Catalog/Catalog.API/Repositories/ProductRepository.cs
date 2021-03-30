using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _catalogContext.Products.Find(p => true).ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _catalogContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.ElemMatch(p => p.Name, name);
            return await _catalogContext.Products.Find(filterDefinition).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filterDefinition =
                Builders<Product>.Filter.ElemMatch(p => p.Category, categoryName);
            return await _catalogContext.Products.Find(filterDefinition).ToListAsync();
        }

        public async Task Create(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> Update(Product product)
        {
            ReplaceOneResult updateResult =
                await _catalogContext.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.ElemMatch(p => p.Id, id);
            DeleteResult deletedProdcut = await _catalogContext.Products.DeleteOneAsync(filterDefinition);

            return deletedProdcut.IsAcknowledged && deletedProdcut.DeletedCount > 0;
        }
    }
}