using Application.DTOs;
using Domain.FiltersDb;

namespace Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<ResultServices<ProductDTO>> CreateAsync(ProductDTO productDTO);
        Task<ResultServices<ProductDTO>> GetByIdAsync(int id);
        Task<ResultServices<ICollection<ProductDTO>>> GetAllAsync();
        Task<ResultServices> UpdateAsynt(ProductDTO productDTO);
        Task<ResultServices> DeleteAsync(int id);
        Task<ResultServices<PagedBaseResponseDTO<ProductDTO>>> GetPagedAsync(FilterDb productFilterDb);
    }
}
