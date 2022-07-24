using Application.DTOs;
using Application.DTOs.Validations;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.FiltersDb;
using Domain.Repositories;

namespace Application.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IProductRepository _productRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMapper _mapper;

        public PurchaseService(IProductRepository productRepository, IPersonRepository personRepository, IPurchaseRepository purchaseRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _personRepository = personRepository;
            _purchaseRepository = purchaseRepository;
            _mapper = mapper;
        }

        public async Task<ResultServices<PurchaseDTO>> CreateAsync(PurchaseDTO purchaseDTO)
        {
            if (purchaseDTO == null)
                return ResultServices.Fail<PurchaseDTO>("Objeto deve ser informado!");

            if (purchaseDTO.QtdProducts.Split(" ").Count() != purchaseDTO.CodErp.Split(" ").Count())
            {
                return ResultServices.Fail<PurchaseDTO>("Informe a quantidade e os valores corretamente!");
            }

            var validation = new PurchaseDTOValidator().Validate(purchaseDTO);
            if (!validation.IsValid)
                return ResultServices.RequestError<PurchaseDTO>("Problemas de validação", validation);

            var list = purchaseDTO.CodErp.Split(" ");
            var personId = await _personRepository.GetIdByDocumentAsync(purchaseDTO.Document);
            var purchase = new Purchase(personId, purchaseDTO.QtdProducts);

            foreach (var item in list)
            {
                purchase.Products.Add(await _productRepository.GetByCodErpAsync(item));
            }

            purchase.ReturnCashBack();

            var data = await _purchaseRepository.CreateAsync(purchase);

            purchaseDTO.Id = data.Id;

            return ResultServices.Ok<PurchaseDTO>(purchaseDTO);
        }

        public async Task<ResultServices<ICollection<PurchaseDetailDTO>>> GetAllAsync()
        {
            var purchases = await _purchaseRepository.GetAllAsync();
            return ResultServices.Ok(_mapper.Map<ICollection<PurchaseDetailDTO>>(purchases));
        }

        public async Task<ResultServices<PurchaseDetailDTO>> GetByIdAsync(int id)
        {
            var purchase = await _purchaseRepository.GetByIdAsync(id);
            if (purchase == null)
                return ResultServices.Fail<PurchaseDetailDTO>("Compra não encontrada!");

            return ResultServices.Ok(_mapper.Map<PurchaseDetailDTO>(purchase));
        }

        public async Task<ResultServices<PurchaseDTO>> UpdateAsync(PurchaseDTO purchaseDTO)
        {
            if (purchaseDTO == null)
                return ResultServices.Fail<PurchaseDTO>("Objeto deve ser informado!");

            var result = new PurchaseDTOValidator().Validate(purchaseDTO);
            if (!result.IsValid)
                return ResultServices.RequestError<PurchaseDTO>("Problemas de validação!", result);

            var purchase = await _purchaseRepository.GetByIdAsync(purchaseDTO.Id);
            var list = purchaseDTO.CodErp.Split(" ");
            List<Product> products = new List<Product>();

            foreach (var item in list)
            {
                products.Add(await _productRepository.GetByCodErpAsync(item));
            }

            var personId = await _personRepository.GetIdByDocumentAsync(purchaseDTO.Document);

            purchase.Edit(purchase.Id, personId, products, purchaseDTO.QtdProducts);
            await _purchaseRepository.EditAsync(purchase);
            return ResultServices.Ok(purchaseDTO);

        }

        public async Task<ResultServices> DeleteAsync(int id)
        {
            var purchase = await _purchaseRepository.GetByIdAsync(id);
            if (purchase == null)
                return ResultServices.Fail("Compra não encontrada!");

            await _purchaseRepository.DeleteAsync(purchase);
            return ResultServices.Ok($"Compra do id: {id} deletada!");
        }

        public async Task<ResultServices<PagedBaseResponseDTO<PurchaseDetailDTO>>> GetPagedAsync(PurchaseFilterDbDTO purchaseFilterDbDTO)
        {
            var purchaseFilterDb = new PurchaseFilterDb();

            if (purchaseFilterDbDTO.InitialDate == null) purchaseFilterDbDTO.InitialDate = new DateTime().ToString();

            if (purchaseFilterDbDTO.LastDate == null) purchaseFilterDbDTO.LastDate =  DateTime.Now.ToString();

            var date = DateTime.Now;

            var validate = DateTime.TryParse(purchaseFilterDbDTO.InitialDate, out date);

            if (!validate) return ResultServices.Fail<PagedBaseResponseDTO<PurchaseDetailDTO>>("Data invalida!");
            purchaseFilterDb.InitialDate = date;

            validate = DateTime.TryParse(purchaseFilterDbDTO.LastDate, out date);

            if (!validate) return ResultServices.Fail<PagedBaseResponseDTO<PurchaseDetailDTO>>("Data invalida!");
            purchaseFilterDb.LastDate = date;

            purchaseFilterDb.OrderByProperty = purchaseFilterDbDTO.OrderByProperty;
            purchaseFilterDb.Page = purchaseFilterDbDTO.Page;
            purchaseFilterDb.PageSize = purchaseFilterDbDTO.PageSize;


            var purchasePaged = await _purchaseRepository.GetPagedAsync(purchaseFilterDb);
            var x = _mapper.Map<List<PurchaseDetailDTO>>(purchasePaged.Data);
            x = x.OrderByDescending(x => x.Date).ToList();
            var result = new PagedBaseResponseDTO<PurchaseDetailDTO>(purchasePaged.TotalRegistes, x);
            return ResultServices.Ok(result);
        }
    }
}
