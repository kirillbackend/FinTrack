using AutoMapper;
using FinTrack.Model;
using FinTrack.Services.Dtos;
using FinTrack.Services.Mappers.Contracts;

namespace FinTrack.Services.Mappers
{
    public class FinanceMapper : AbstractMapper<Finance, FinanceDto>, IFinanceMapper
    {
        protected override AutoMapper.IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Finance, FinanceDto>().ReverseMap();
            });
            return config.CreateMapper();
        }
    }
}
