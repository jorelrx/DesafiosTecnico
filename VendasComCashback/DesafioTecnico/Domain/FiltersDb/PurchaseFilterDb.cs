using Domain.Repositories;

namespace Domain.FiltersDb
{
    public class PurchaseFilterDb : PagedBaseRequest
    {
        public DateTime InitialDate { get; set; }
        public DateTime LastDate { get; set; }
    }
}
