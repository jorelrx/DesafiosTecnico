using Domain.Validations;

namespace Domain.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public double CashBack { get; set; }
        public DateTime Date { get; set; }
        public Person Person { get; set; }
        public ICollection<Product> Products { get; set; }
        public string QtdProduct { get; set; }

        public Purchase(int personId, string qtdProduct)
        {
            Validation(personId, qtdProduct);
            Date = DateTime.Now;
            Products = new List<Product>();
        }
        public Purchase(int id, int personId, string qtdProduct)
        {
            DomainValidationException.When(id < 0, "Id deve ser informado!");
            Id = id;
            Validation(personId, qtdProduct);
            Products = new List<Product>();
        }

        public void Edit(int id, int personId, ICollection<Product> products, string qtdProduct)
        {
            DomainValidationException.When(id <= 0, "Id deve ser informado!");
            Id = id;
            Validation(personId, qtdProduct);
            Products = products;
            ReturnCashBack();
        }

        private void Validation(int personId, string qtdProduct)
        {
            DomainValidationException.When(personId <= 0, "Id pessoa deve ser informado!");
            DomainValidationException.When(string.IsNullOrEmpty(qtdProduct), "Quantidade de produtos deve ser informado!");
            PersonId = personId;
            QtdProduct = qtdProduct;
        }
        public void ReturnCashBack()
        {
            var date = Date.ToString("ddd");
            double valorTotal = new();
            int i = 0;
            foreach (Product product in Products)
            {
                string[] list = product.ValueCashBack.Split(" ");
                var valor = date switch
                {
                    "dom" => product.Price * double.Parse(list[0]) / 100,
                    "seg" => product.Price * double.Parse(list[1]) / 100,
                    "ter" => product.Price * double.Parse(list[2]) / 100,
                    "qua" => product.Price * double.Parse(list[3]) / 100,
                    "qui" => product.Price * double.Parse(list[4]) / 100,
                    "sex" => product.Price * double.Parse(list[5]) / 100,
                    "sáb" => product.Price * double.Parse(list[6]) / 100,
                    _ => 0,
                };

                valorTotal += valor * double.Parse(QtdProduct.Split(" ")[i++]);
            }
            CashBack = valorTotal;
        }
    }
}
