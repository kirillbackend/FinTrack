using AutoMapper;
using FinTrack.Model;
using FinTrack.Services.Dtos;
using FinTrack.Services.Mappers.Contracts;

namespace FinTrack.Services.Mappers
{
    public class UserMapper : AbstractMapper<User, UserDto>, IUserMapper
    {
        protected override AutoMapper.IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDto, User>();

                cfg.CreateMap<User, UserDto>()
                 .ForMember(x => x.Password, y => y.Ignore());
            });
            return config.CreateMapper();
        }
    }
}
