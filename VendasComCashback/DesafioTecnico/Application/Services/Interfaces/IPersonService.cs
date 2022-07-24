using Application.DTOs;
using Domain.FiltersDb;

namespace Application.Services.Interfaces
{
    public interface IPersonService
    {
        Task<ResultServices<PersonDTO>> CreateAsync(PersonDTO personDTO);
        Task<ResultServices<ICollection<PersonDTO>>> GetAllAsync();
        Task<ResultServices<PersonDTO>> GetByIdAsync(int id);
        Task<ResultServices> UpdateAsync(PersonDTO personDTO);
        Task<ResultServices> DeleteAsync(int id);
        Task<ResultServices<PagedBaseResponseDTO<PersonDTO>>> GetPagedAsync(FilterDb personFilterDb);
    }
}
