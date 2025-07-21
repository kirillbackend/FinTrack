using AutoMapper;
using FinTrack.Model;
using FinTrack.Services.Dtos;
using FinTrack.Services.Mappers.Contracts;

namespace FinTrack.Services.Mappers
{
    public class CategoryMapper : AbstractMapper<Category, CategoryDto>, ICategoryMapper
    {
        protected override AutoMapper.IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryDto>().ReverseMap();
            });
            return config.CreateMapper();

        }
    }
}
