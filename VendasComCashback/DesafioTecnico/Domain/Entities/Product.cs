using Domain.Validations;

namespace Domain.Entities
{
    public sealed class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CodErp { get; set; }
        public double Price { get; set; }
        public string ValueCashBack { get; set; }
        public ICollection<Purchase> Purchases { get; set; }

        public Product(string name, string codErp, double price, string valueCashBack)
        {
            Validation(name, codErp, price);
            ValueCashBack = valueCashBack;
            Purchases = new List<Purchase>();
        }

        public Product(int id, string name, string codErp, double price, string valueCashBack)
        {
            DomainValidationException.When(id < 0, "Produto deve ser informado!");
            Id = id;
            Validation(name, codErp, price);
            ValueCashBack = valueCashBack;
        }

        private void Validation(string name, string codErp, double price)
        {
            DomainValidationException.When(string.IsNullOrEmpty(name), "Nome deve ser informado!");
            DomainValidationException.When(string.IsNullOrEmpty(codErp), "Código deve ser informado!");
            DomainValidationException.When(price < 0, "Informe um preço valido!");

            Name = name;
            CodErp = codErp;
            Price = price;
        }
    }
}
