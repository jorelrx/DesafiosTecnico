using Domain.Entities;
using Domain.FiltersDb;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);
        Task<ICollection<Product>> GetProductsAsync();
        Task<Product> CreateAsync(Product product);
        Task EditAsync(Product product);
        Task DeleteAsync(Product product);
        Task<Product> GetByCodErpAsync(string codErp);
        Task<PagedBaseResponse<Product>> GetPagedAsync(FilterDb request);
    }
}
