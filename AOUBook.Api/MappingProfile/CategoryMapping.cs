using AOUBook.Api.Models;
using AOUBook.Models;
using AutoMapper;

namespace AOUBook.Api.MappingProfile
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<Category,CategoryResponse>().ReverseMap();
        }
    }
}
