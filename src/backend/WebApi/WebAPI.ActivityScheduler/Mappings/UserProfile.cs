using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Presentation.EntitiesDTO;
using AutoMapper;


namespace WebAPI.ActivityScheduler.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserToRegisterDTO, User>().ReverseMap();
            CreateMap<UserToLoginDTO, User>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
        }
    }
}
