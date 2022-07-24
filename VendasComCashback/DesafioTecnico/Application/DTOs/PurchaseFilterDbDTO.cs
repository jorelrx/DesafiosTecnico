using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PurchaseFilterDbDTO : PagedBaseRequest
    {
        public string? InitialDate { get; set; }
        public string? LastDate { get; set; }
    }
}
