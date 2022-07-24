using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PurchaseDetailDTO
    {
        public int Id { get; set; }
        public string NamePerson { get; set; }
        public string[] NameProducts { get; set; }
        public string[] QtdProduct { get; set; }
        public double CashBack { get; set; }
        public DateTime Date { get; set; }
    }
}
