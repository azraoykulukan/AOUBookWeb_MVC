using AOUBook.Api.Models;
using AOUBook.Models;
using AutoMapper;

namespace AOUBook.Api.MappingProfile
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductResponse>().ReverseMap();
        }
    }
}
