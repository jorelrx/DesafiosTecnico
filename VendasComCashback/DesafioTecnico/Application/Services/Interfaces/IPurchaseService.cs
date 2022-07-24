using Application.DTOs;
using Domain.FiltersDb;

namespace Application.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<ResultServices<PurchaseDTO>> CreateAsync(PurchaseDTO purchaseDTO);
        Task<ResultServices<PurchaseDetailDTO>> GetByIdAsync(int id);
        Task<ResultServices<ICollection<PurchaseDetailDTO>>> GetAllAsync();
        Task<ResultServices<PurchaseDTO>> UpdateAsync(PurchaseDTO purchaseDTO);
        Task<ResultServices> DeleteAsync(int id);
        Task<ResultServices<PagedBaseResponseDTO<PurchaseDetailDTO>>> GetPagedAsync(PurchaseFilterDbDTO purchaseFilterDbDTO);
    }
}
