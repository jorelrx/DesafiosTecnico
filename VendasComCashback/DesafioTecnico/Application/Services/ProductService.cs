using Application.DTOs;
using Application.DTOs.Validations;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.FiltersDb;
using Domain.Repositories;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ResultServices<ProductDTO>> CreateAsync(ProductDTO productDTO)
        {
            if (productDTO == null)
                return ResultServices.Fail<ProductDTO>("Objeto deve ser informado!");

            if (productDTO.ValueCashBack.Split(" ").Count() < 7)
                return ResultServices.Fail<ProductDTO>("Preencha o campo de cashback corretamente. Ex: 10 10 10 10 10 10 10");

            var result = new ProductDTOValidator().Validate(productDTO);
            if (!result.IsValid)
                return ResultServices.RequestError<ProductDTO>("Problema na validação", result);

            var product = _mapper.Map<Product>(productDTO);
            var data = await _productRepository.CreateAsync(product);
            return ResultServices.Ok<ProductDTO>(_mapper.Map<ProductDTO>(data));
        }

        public async Task<ResultServices> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return ResultServices.Fail("Produto não encontrado!");

            await _productRepository.DeleteAsync(product);
            return ResultServices.Ok($"Produto do id {id} foi deletado!");
        }

        public async Task<ResultServices<ICollection<ProductDTO>>> GetAllAsync()
        {
            var product = await _productRepository.GetProductsAsync();
            return ResultServices.Ok<ICollection<ProductDTO>>(_mapper.Map<ICollection<ProductDTO>>(product));
        }

        public async Task<ResultServices<ProductDTO>> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return ResultServices.Fail<ProductDTO>("Produto não encontrado!");

            return ResultServices.Ok<ProductDTO>(_mapper.Map<ProductDTO>(product));
        }

        public async Task<ResultServices> UpdateAsynt(ProductDTO productDTO)
        {
            if (productDTO == null)
                return ResultServices.Fail("Objeto deve ser informado!");

            var validation = new ProductDTOValidator().Validate(productDTO);
            if (!validation.IsValid)
                return ResultServices.RequestError("Problemas de validação!", validation);

            var product = await _productRepository.GetByIdAsync(productDTO.Id);
            if (product == null)
                return ResultServices.Fail("Produto não encontrado");

            product = _mapper.Map<ProductDTO, Product>(productDTO, product);
            await _productRepository.EditAsync(product);
            return ResultServices.Ok("Produto editado!");
        }

        public async Task<ResultServices<PagedBaseResponseDTO<ProductDTO>>> GetPagedAsync(FilterDb productFilterDb)
        {
            productFilterDb.OrderByProperty = "Name";
            var productPaged = await _productRepository.GetPagedAsync(productFilterDb);
            var result = new PagedBaseResponseDTO<ProductDTO>(productPaged.TotalRegistes, _mapper.Map<List<ProductDTO>>(productPaged.Data));

            return ResultServices.Ok(result);
        }
    }
}
