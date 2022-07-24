using AutoMapper;
using Application.DTOs;
using Domain.Entities;

namespace Application.Mappings
{
    public class DomainToDtoMapping : Profile
    {
        public DomainToDtoMapping()
        {
            CreateMap<Person, PersonDTO>();
            CreateMap<Product, ProductDTO>();
            CreateMap<Purchase, PurchaseDetailDTO>()
                .ForMember(x => x.NamePerson, opt => opt.Ignore())
                .ForMember(x => x.NameProducts, opt => opt.Ignore())
                .ForMember(x => x.QtdProduct, opt => opt.Ignore())
                .ConstructUsing((model, context) =>
                {
                    var dto = new PurchaseDetailDTO
                    {
                        NamePerson = model.Person.Name,
                        Id = model.Id,
                        NameProducts = model.Products.Select(x => x.Name).ToArray(),
                        QtdProduct = model.QtdProduct.Split(" "),
                        CashBack = model.CashBack,
                        Date = model.Date

                    };
                    return dto;
                });
        }
    }
}
