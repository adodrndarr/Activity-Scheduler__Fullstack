using AutoMapper;
using WebAPI.ActivityScheduler.Entities;
using WebAPI.ActivityScheduler.EntitiesDTO;
using WebAPI.ActivityScheduler.EntitiesDTO.Login;
using WebAPI.ActivityScheduler.EntitiesDTO.ManageUsers;
using WebAPI.ActivityScheduler.EntitiesDTO.Registration;


namespace WebAPI.ActivityScheduler.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserToRegisterDTO, User>().ReverseMap();
            CreateMap<UserToLoginDTO, User>().ReverseMap();

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Activity, ActivityDTO>().ReverseMap();
        }
    }
}
