using Domain.Repositories;

namespace Domain.FiltersDb
{
    public class FilterDb : PagedBaseRequest
    {
        public string? Name { get; set; }
    }
}
