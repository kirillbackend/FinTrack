using AutoMapper;
using FinTrack.Model;
using FinTrack.Services.Dtos;
using FinTrack.Services.Mappers.Contracts;

namespace FinTrack.Services.Mappers
{
    public class CurrencyMapper : AbstractMapper<Currency, CurrencyDto>, ICurrencyMapper
    {
        protected override AutoMapper.IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Currency, CurrencyDto>().ReverseMap();
            });
            return config.CreateMapper();
        }
    }
}
