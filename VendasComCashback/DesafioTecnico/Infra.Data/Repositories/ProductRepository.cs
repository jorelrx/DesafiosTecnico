using Domain.Entities;
using Domain.FiltersDb;
using Domain.Repositories;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ICollection<Product>> GetProductsAsync()
        {
            return await _db.Products.ToListAsync();
        }
        public async Task<Product> CreateAsync(Product product)
        {
            _db.Add(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(Product product)
        {
            _db.Remove(product);
            await _db.SaveChangesAsync();
        }

        public async Task EditAsync(Product product)
        {
            _db.Update(product);
            await _db.SaveChangesAsync();
        }

        public async Task<Product> GetByCodErpAsync(string codErp)
        {
            return await _db.Products.FirstOrDefaultAsync(x => x.CodErp == codErp);
        }

        public async Task<PagedBaseResponse<Product>> GetPagedAsync(FilterDb request)
        {
            var product = _db.Products.AsQueryable();
            if (!string.IsNullOrEmpty(request.Name))
                product = product.Where(x => x.Name.Contains(request.Name));

            return await PagedBaseResponseHelper
                .GetResponseAsync<PagedBaseResponse<Product>, Product>(product, request);
        }
    }
}
